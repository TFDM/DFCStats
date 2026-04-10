using DFCStats.Data;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Participants;
using Microsoft.EntityFrameworkCore;
using DFCStats.Business.MappingExtensions;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Business
{
    public class ParticipationService : IParticipationService
    {
        private readonly DFCStatsDBContext _dfcStatsDbContext;
        private readonly IFixtureService _fixtureService;

        public ParticipationService(DFCStatsDBContext dFCStatsDBContext, IFixtureService fixtureService)
        {
            _dfcStatsDbContext = dFCStatsDBContext;
            _fixtureService = fixtureService;
        }

        /// <summary>
        /// Gets a participation by id, including related fixture, season and person details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ParticipationDTO?> GetParticipationByIdAsync(Guid id)
        {
            var query = _dfcStatsDbContext.Participants.AsNoTracking().AsQueryable();

            query = query.Include(p => p.Fixture).ThenInclude(f => f!.Season)
                .Include(p => p.Fixture).ThenInclude(f => f!.Club)
                .Include(p => p.Fixture).ThenInclude(f => f!.Venue)
                .Include(p => p.Person);

            var participation = await query.FirstOrDefaultAsync(p => p.Id == id);
            return participation?.MapToParticipantsDTO();
        }

        /// <summary>
        /// Adds a participation record to the database
        /// </summary>
        /// <param name="newParticipationDTO"></param>
        /// <returns></returns>
        public async Task<ParticipationDTO> AddParticipationAsync(ParticipationDTO newParticipationDTO)
        {
            // Sets the values of the started and substitute variables based on the role in the DTO
			var (started, substitute) = SetRoles(newParticipationDTO.Role);

            // Checks the the person isn't replacing themselves
			CheckPersonIsNotReplacingThemselves(newParticipationDTO.PersonId, newParticipationDTO.ReplacedByPersonId);

            // If the person started the fixture then check that there aren't already 11 players marked as starting already
            if (started)
                await CheckNumberOfStartingPlayers(newParticipationDTO.FixtureId);

            // Set the order number for the participation record
            var orderNumber = await SetOrderNumber(newParticipationDTO.FixtureId);

            // Check that the person isn't already involved in the fixture
            await CheckPersonNotAlreadyInvolvedInFixture(newParticipationDTO.FixtureId, newParticipationDTO.PersonId);

            // Sets the replaced time to null if its zero
			if (newParticipationDTO.ReplacedByTime == 0)
				newParticipationDTO.ReplacedByTime = null;

            // Add the participation record to the database
            var participation = new Participation
            {
                FixtureId = newParticipationDTO.FixtureId,
                PersonId = newParticipationDTO.PersonId,
                Started = started,
                Sub = substitute,
                YellowCard = newParticipationDTO.YellowCard,
                RedCard = newParticipationDTO.RedCard,
                ReplacedByPersonId = newParticipationDTO.ReplacedByPersonId,
                ReplacedTime = newParticipationDTO.ReplacedByTime,
                OrderNo = orderNumber
            };

            // Save the participation record to the database
             _dfcStatsDbContext.Participants.Add(participation);
             await _dfcStatsDbContext.SaveChangesAsync();

            return participation.MapToParticipantsDTO()!;
        }

        /// <summary>
        /// Returns a tuple of bool values to based on the role in the fixture
        /// </summary>
        /// <param name="roleInFixture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private (bool, bool) SetRoles(string roleInFixture)
		{
			// Sets the started and sub values based on the role
			switch (roleInFixture)
			{
				case "Starting XI":
					// Person started the fixture
					return (true, false);
				case "Substitute":
					// Person was a substitute
					return (false, true);
				default:
					// Throw an exception as the role in fixture valid was an unexpected value
					throw new ArgumentException(String.Format("{0} is not a valid role", roleInFixture));
			}
		}

        /// <summary>
        /// Checks that the person isn't replacing themselves by comparing the person id and replaced by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="replacedByPersonId"></param>
        /// <exception cref="Exception"></exception>
        private void CheckPersonIsNotReplacingThemselves(Guid personId, Guid? replacedByPersonId)
        {
            if (personId == replacedByPersonId)
            {
                // Throw an exception as a person can't replaced by themselves
                throw new DFCStatsException("This person can't be replaced by themselves");
            }
        }

        /// <summary>
        /// Checks that there aren't already 11 players marked as starting the fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <param name="participantId"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        private async Task CheckNumberOfStartingPlayers(Guid fixtureId, Guid? participantId = null)
		{
            // Get the fixture and the participants 
			var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);
            
            if (fixture == null)
                // Throw an exception as the fixture couldn't be found
                throw new DFCStatsException("Fixture not found");

            // If a participant Id has been supplied then this is used to exclude the exisiting 
			// participant record from the number of players checked - this ensures that if a player
			// who started a fixture and is being updated doesn't hit the 11 starters limit
			if (participantId != null)
			{
                // Check if there are already 11 players marked as starting the fixture excluding the supplied participant
                if(fixture.Participants!.Where(p => p.Started && p.Id != participantId).Count() == 11)
                {
                    throw new DFCStatsException("There are already 11 people in the database who started this fixture");
                }

				return;
            }

            if (fixture.Participants!.Where(p => p.Started).Count() == 11)
            {
                // Throw an exception as there are currently already 11 starting players attached to the fixture
                throw new DFCStatsException("There are already 11 people in the database who started this fixture");
            }
		}

        /// <summary>
        /// Returns an order number for the participation record based on the other participants attached to the fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        private async Task<int> SetOrderNumber(Guid fixtureId)
		{
            // Get the fixture and the participants 
			var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);

            // Check if the fixture was found
            if (fixture == null)
                // Throw an exception as the fixture couldn't be found
                throw new DFCStatsException("Fixture not found");

            // If there are no participants attached to the fiture then return 0 as the order number
            if (fixture.Participants!.Count == 0)
                return 0;

            // Get the highest order number from the participants attached to the fixture and add 1 to it for the new participant
            var orderNumber = fixture.Participants.OrderByDescending(p => p.OrderNo).Select(p => p.OrderNo).First();
            return orderNumber + 1;
		}

        /// <summary>
        /// Checks that the person isn't already involved in the fixture
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <param name="personId"></param>
        /// <param name="participantId"></param>
        /// <returns></returns>
        /// <exception cref="DFCStatsException"></exception>
        private async Task CheckPersonNotAlreadyInvolvedInFixture(Guid fixtureId, Guid personId, Guid? participantId = null)
		{
            // Get the fixture
            var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);

            // Check if the fixture was found
            if (fixture == null)
                // Throw an exception as the fixture couldn't be found
                throw new DFCStatsException("Fixture not found");

            // If a participant Id has been supplied then this is used to exclude the exisiting participant record from the check
            if (participantId != null)
			{
                if (fixture.Participants!.Where(p => p.Id != participantId && p.PersonId == personId).SingleOrDefault() != null)
                {
                    // Throw an exception as this person is already involved in the fixture
                    throw new DFCStatsException("This person is already involved in this fixture");
                }
			}

			if (fixture.Participants!.Where(p => p.PersonId == personId).SingleOrDefault() != null)
			{
                // Throw an exception as this person is already involved in the fixture
				throw new DFCStatsException("This person is already involved in this fixture");
			}
		}


    }
}