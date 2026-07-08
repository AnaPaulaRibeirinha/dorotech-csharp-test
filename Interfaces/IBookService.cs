using DoroTech.Bookstore.Api.DTO;

namespace DoroTech.Bookstore.Api.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDto>> GetAllAsync(string? search, int page, int pageSize);
        Task<BookResponseDto?> GetByIdAsync(int id);
        Task<BookResponseDto> CreateAsync(BookCreateDto dto);
        Task<bool> UpdateAsync(int id, BookUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
