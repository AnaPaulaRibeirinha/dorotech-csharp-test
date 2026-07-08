using DoroTech.Bookstore.Api.DTO;
using DoroTech.Bookstore.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoroTech.Bookstore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService service, ILogger<BooksController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Consulta livros cadastrados com ordenação ascendente por título.
        /// Acesso público.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Página e tamanho da página devem ser maiores que zero.");

            var books = await _service.GetAllAsync(search, page, pageSize);

            return Ok(books);
        }

        /// <summary>
        /// Consulta um livro pelo ID.
        /// Acesso público.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);

            if (book == null)
                return NotFound("Livro não encontrado.");

            return Ok(book);
        }

        /// <summary>
        /// Cadastra um novo livro.
        /// Requer autenticação de administrador.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(BookCreateDto dto)
        {
            try
            {
                var book = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar livro.");
                return StatusCode(500, "Erro interno ao cadastrar livro.");
            }
        }

        /// <summary>
        /// Atualiza um livro existente.
        /// Requer autenticação de administrador.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Livro não encontrado.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar livro.");
                return StatusCode(500, "Erro interno ao atualizar livro.");
            }
        }

        /// <summary>
        /// Exclui um livro.
        /// Requer autenticação de administrador.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);

                if (!deleted)
                    return NotFound("Livro não encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir livro.");
                return StatusCode(500, "Erro interno ao excluir livro.");
            }
        }
    }
}
