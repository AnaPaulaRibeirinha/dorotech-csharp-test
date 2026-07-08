namespace DoroTech.Bookstore.Api.DTO
{
    public class BookResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string? Isbn { get; set; }

        public int PublicationYear { get; set; }

        public int Quantity { get; set; }
    }
}
