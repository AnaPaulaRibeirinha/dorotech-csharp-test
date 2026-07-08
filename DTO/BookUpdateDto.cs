using System.ComponentModel.DataAnnotations;

namespace DoroTech.Bookstore.Api.DTO
{
    public class BookUpdateDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Author { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Isbn { get; set; }

        [Range(1, 9999)]
        public int PublicationYear { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
