using System.ComponentModel.DataAnnotations;
using static GCommon.GlobalValidation;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Web.ViewModels.Event
{
    public class EventCreateInputModel
    {
        [Required(ErrorMessage = "The field is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The field is required.")]
        public int CategoryId { get; set; }

        [Required]
        [Range(MinPrice, MaxPrice, ErrorMessage = "The price must be between 0.01 and 1000.")]
        public decimal TicketPrice { get; set; }

        [Required]
        [Range(MinTickets, MaxTickets, ErrorMessage = "The number of tickets must be between 1 and 10 000.")]
        public int TotalTickets { get; set; }
        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;
        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(CreatedOnLength, MinimumLength = CreatedOnLength)]
        public string CreatedOn { get; set; } = null!;

        public virtual IEnumerable<AddCategoryDropDownModel>? Categories { get; set; }
    }
}
