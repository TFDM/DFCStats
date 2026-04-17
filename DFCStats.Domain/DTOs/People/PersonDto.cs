using DFCStats.Domain.DTOs.Seasons;
using DFCStats.Domain.DTOs.Appearances;

namespace DFCStats.Domain.DTOs.People
{
    public class PersonDTO
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastNameFirstName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Nationality { get; set; }
        public string? NationalityIcon { get; set; }
        public string? Biography { get; set; }
        public int? TotalApps { get; set; }
        public int? TotalGoals { get; set; }
        public int? TotalRedCards { get; set; }
        public int? TotalStarts { get; set; }
        public int? TotalSubs { get; set; }
        public int? TotalLeagueStarts { get; set; }
        public int? TotalLeagueSubs { get; set; }
        public int? TotalLeagueGoals { get; set; }
        public int? TotalPlayOffStarts { get; set; }
        public int? TotalPlayOffSubs { get; set; }
        public int? TotalPlayOffGoals { get; set; }
        public int? TotalCupStarts { get; set; }
        public int? TotalCupSubs { get; set; }
        public int? TotalCupGoals { get; set; }
        public decimal? GoalsPerGame { get; set; }
        public List<SeasonShortDTO>? Seasons { get; set; }
        public List<SeasonalAppearanceDTO>? Appearances { get; set; }
        public bool IsManager { get; set; }
    }

    public class NewPersonDTO
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Biography { get; set; }
        public bool IsManager { get; set; }
        public List<Guid>? ListOfSeasons { get; set; }
    }

    public class EditPersonDTO
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public Guid? NationalityId { get; set; }
        public string? Biography { get; set; }
        public bool IsManager { get; set; }
        public List<Guid>? ListOfSeasons { get; set; }
    }
}