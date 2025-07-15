using System.ComponentModel.DataAnnotations;
using TicketManager.Web.ViewModels.Event;
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
        [Range(MinPrice, MaxPrice, ErrorMessage = "Ticket price must be greater than 0.")]
        public decimal TicketPrice { get; set; }

        [Required]
        [Range(MinTickets, MaxTickets, ErrorMessage = "Total tickets must be at least 1.")]
        public int TotalTickets { get; set; }
        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(CreatedOnLength, MinimumLength = CreatedOnLength)]
        public string CreatedOn { get; set; } = null!;

        public virtual IEnumerable<AddCategoryDropDownModel>? Categories { get; set; }
    }
}