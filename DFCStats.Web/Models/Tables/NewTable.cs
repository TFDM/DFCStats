using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Models.Tables
{
    public class NewTable
    {
        public Guid SeasonId { get; set; }
        [DisplayName("Club")]
		public Guid? ClubId { get; set; }
        public int? Played { get; set; }
        [DisplayName("HW")]
        public int? HomeWon { get; set; }
        [DisplayName("HD")]
        public int? HomeDrawn { get; set; }
        [DisplayName("HL")]
        public int? HomeLost { get; set; }
        [DisplayName("HGF")]
        public int? HomeGoalsFor { get; set; }
        [DisplayName("HGA")]
        public int? HomeGoalsAgainst { get; set; }
        [DisplayName("AW")]
        public int? AwayWon { get; set; }
        [DisplayName("AD")]
        public int? AwayDrawn { get; set; }
        [DisplayName("AL")]
        public int? AwayLost { get; set; }
        [DisplayName("AGF")]
        public int? AwayGoalsFor { get; set; }
        [DisplayName("AGA")]
        public int? AwayGoalsAgainst { get; set; }
        [DisplayName("Pts")]
        public int? Points { get; set; }
        [DisplayName("Champion")]
        public bool? IsChampion { get; set; }
        [DisplayName("Promotion")]
        public bool? IsPromotion { get; set; }
        [DisplayName("Play-Offs")]
        public bool? IsPlayOffs { get; set; }
        [DisplayName("Relegation")]
        public bool? IsRelegated { get; set; }
        [DisplayName("Is Darlington?")]
        public bool? IsDarlington { get; set; }
        public string? Notes { get; set; }

        // Holds the list of options
        public IEnumerable<SelectListItem> ClubOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> IsChampionOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> IsPromotionOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> IsPlayOffsOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> IsRelegatedOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> IsDarlingtonOptions { get; set; } = new List<SelectListItem>();
    }
}