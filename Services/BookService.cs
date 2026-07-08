using DoroTech.Bookstore.Api.DTO;
using DoroTech.Bookstore.Api.Entities;
using DoroTech.Bookstore.Api.Interfaces;

namespace DoroTech.Bookstore.Api.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository repository, ILogger<BookService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllAsync(string? search, int page, int pageSize)
        {
            var books = await _repository.GetAllAsync(search, page, pageSize);

            return books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Isbn = b.Isbn,
                PublicationYear = b.PublicationYear,
                Quantity = b.Quantity
            });
        }

        public async Task<BookResponseDto?> GetByIdAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);

            if (book == null)
                return null;

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Isbn = book.Isbn,
                PublicationYear = book.PublicationYear,
                Quantity = book.Quantity
            };
        }

        public async Task<BookResponseDto> CreateAsync(BookCreateDto dto)
        {
            if (await _repository.ExistsByTitleAsync(dto.Title))
            {
                _logger.LogWarning("Tentativa de cadastrar livro duplicado: {Title}", dto.Title);
                throw new InvalidOperationException("Livro já cadastrado.");
            }

            var book = new Book
            {
                Title = dto.Title.Trim(),
                Author = dto.Author.Trim(),
                Isbn = dto.Isbn?.Trim(),
                PublicationYear = dto.PublicationYear,
                Quantity = dto.Quantity
            };

            await _repository.AddAsync(book);

            _logger.LogInformation("Livro cadastrado com sucesso: {Title}", book.Title);

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Isbn = book.Isbn,
                PublicationYear = book.PublicationYear,
                Quantity = book.Quantity
            };
        }

        public async Task<bool> UpdateAsync(int id, BookUpdateDto dto)
        {
            var book = await _repository.GetByIdAsync(id);

            if (book == null)
                return false;

            if (await _repository.ExistsByTitleAsync(dto.Title, id))
            {
                _logger.LogWarning("Tentativa de atualizar livro para título duplicado: {Title}", dto.Title);
                throw new InvalidOperationException("Já existe outro livro com este título.");
            }

            book.Title = dto.Title.Trim();
            book.Author = dto.Author.Trim();
            book.Isbn = dto.Isbn?.Trim();
            book.PublicationYear = dto.PublicationYear;
            book.Quantity = dto.Quantity;
            book.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(book);

            _logger.LogInformation("Livro atualizado: {Id}", id);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);

            if (book == null)
                return false;

            await _repository.DeleteAsync(book);

            _logger.LogInformation("Livro excluído: {Id}", id);

            return true;
        }
    }
}
