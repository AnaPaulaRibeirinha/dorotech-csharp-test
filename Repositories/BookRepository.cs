using DoroTech.Bookstore.Api.Data;
using DoroTech.Bookstore.Api.Entities;
using DoroTech.Bookstore.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoroTech.Bookstore.Api.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(string? search, int page, int pageSize)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b =>
                    b.Title.Contains(search) ||
                    b.Author.Contains(search) ||
                    (b.Isbn != null && b.Isbn.Contains(search)));
            }

            return await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> GetByTitleAsync(string title)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByTitleAsync(string title, int? ignoreId = null)
        {
            return await _context.Books.AnyAsync(b =>
                b.Title.ToLower() == title.ToLower() &&
                (!ignoreId.HasValue || b.Id != ignoreId.Value));
        }
    }
}
