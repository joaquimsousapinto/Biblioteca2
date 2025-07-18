using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca2.Models;

namespace Biblioteca2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController(Biblioteca2Context context) : ControllerBase
    {

        // GET: api/Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO_AutorIndex>>> GetAutors()
        {
            return await context.Autors
                .Include(x => x.LivroAutors).ThenInclude(x => x.Livro)
                .Select(x => new DTO_AutorIndex
                {
                    Id = x.Id,
                    Nome = x.Nome
                })
                .ToListAsync();
        }

        // GET: api/Autores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await context.Autors.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            DTO_Autor dTO_Autor = new()
            {
                Id = id,
                Nome = autor.Nome,
                Bibliografia = autor.Bibliografia,
                DataNascimento = autor.DataNascimento.HasValue ? autor.DataNascimento.Value.ToString("yyyy-MM-dd") : string.Empty,
                DataMorte = autor.DataMorte.HasValue ? autor.DataMorte.Value.ToString("yyyy-MM-dd") : string.Empty,
                Livros = [.. autor.LivroAutors.Select(l => new DTO_LivroIndex()
                {
                    Id = l.Livro.Id,
                    Titulo = l.Livro.Titulo
                })]
            };
            return Ok(dTO_Autor);
        }
    }

    public class DTO_AutorIndex
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
    }
    public class DTO_Autor
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public string? Bibliografia { get; set; }
        public string? DataNascimento { get; set; }
        public string? DataMorte { get; set; }
        public List<DTO_LivroIndex>? Livros { get; set; }
    }

    public class DTO_LivroIndex
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
    }
}
