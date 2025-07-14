
using System.ComponentModel.DataAnnotations;
using static GCommon.GlobalValidation.Event;

namespace TicketManager.Web.ViewModels.Event
{
    public class EventEditInputModel
    {
        public int Id { get; set; }  // за редакция, при създаване е 0

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength,  MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string CreatedOn { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal TicketPrice { get; set; }

        [Required]
        [Range(1, 100000)]
        public int TotalTickets { get; set; }

        [Required]
        public int CategoryId { get; set; }


        // Dropdown за избор на категория
        public IEnumerable<AddCategoryDropDownModel>? Categories { get; set; }
    }
}