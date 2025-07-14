using System.ComponentModel.DataAnnotations;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Web.ViewModels.Event
{
    public class EventCreateInputModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }

        [Required]
        [Range(MinPrice, MaxPrice)]
        public decimal TicketPrice { get; set; }

        [Required]
        [Range(MinTickets, MaxTickets)]
        public int TotalTickets { get; set; }
        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(CreatedOnLength, MinimumLength = CreatedOnLength)]
        public string CreatedOn { get; set; } = null!;

        public IEnumerable<AddCategoryDropDownModel>? Categories { get; set; }
    }
}
