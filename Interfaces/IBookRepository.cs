using DoroTech.Bookstore.Api.Entities;

namespace DoroTech.Bookstore.Api.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(string? search, int page, int pageSize);
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByTitleAsync(string title);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<bool> ExistsByTitleAsync(string title, int? ignoreId = null);

    }
}
