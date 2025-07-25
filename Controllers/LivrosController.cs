using Biblioteca2.Models;
using Biblioteca2.Models.Common;
using Biblioteca2.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.EF;

namespace Biblioteca2.Controllers
{
    public class LivrosController(Biblioteca2Context context, IOptions<AppSettings> appSettings) : Controller
    {
        private readonly Biblioteca2Context _context = context;
        private AppSettings AppSettings { get; } = appSettings.Value;

        // GET: Livros
        public async Task<IActionResult> Index(int? page, string q)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int pageSize = AppSettings.ItensPorPagina;
            int pageNumber = page ?? 1;
            ViewData["q"] = q;
            IPagedList<Livro> retVal;
            if (!string.IsNullOrEmpty(q))
            {
                retVal = await _context.Livros
                    .Where(x => x.Titulo.Contains(q))
                    .Include(x => x.Editora)
                    .OrderBy(x => x.Titulo)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                retVal = await _context.Livros
                    .Include(x => x.Editora)
                    .OrderBy(x => x.Titulo)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            stopwatch.Stop();
            ViewData["ElapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;

            return View(retVal);
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.Editora).Include(l => l.Categoria).Include(l => l.LivroAutors).ThenInclude(la => la.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }
            var autores = livro.LivroAutors.Select(la => la.Autor.Id).ToList();
            ViewData["Autores"] = new SelectList(_context.Autors.Where(a => !autores.Contains(a.Id)).OrderBy(a => a.Nome).ToList(), "Id", "Nome");
            var categorias = livro.Categoria.Select(c => c.Id).ToList();
            ViewData["Categorias"] = new SelectList(_context.Categoria.Where(c => !categorias.Contains(c.Id)).OrderBy(c => c.Nome).ToList(), "Id", "Nome");
            return View(livro);
        }

        // GET: Livros/Create
        public IActionResult Create()
        {
            ViewData["EditoraId"] = new SelectList(_context.Editoras.OrderBy(x => x.Nome), "Id", "Nome");
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,AnoPublicacao,EditoraId,AnoEdicao,Exemplares")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na criação do Registo.";
            ViewData["EditoraId"] = new SelectList(_context.Editoras.OrderBy(x => x.Nome), "Id", "Nome", livro.EditoraId);
            return View(livro);
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            ViewData["EditoraId"] = new SelectList(_context.Editoras.OrderBy(x => x.Nome), "Id", "Nome", livro.EditoraId);
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,AnoPublicacao,EditoraId,AnoEdicao,Exemplares")] Livro livro)
        {
            if (id != livro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(livro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo editado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na edição do Registo.";
            ViewData["EditoraId"] = new SelectList(_context.Editoras.OrderBy(x => x.Nome), "Id", "Nome", livro.EditoraId);
            return View(livro);
        }

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.Editora)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
                TempData["Success"] = "Registo apagado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro a apagar o Registo.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("_AdicionarAutor")]
        public async Task<IActionResult> AddAutor(int LivroId, [Bind("LivroId, AutorId, OrdemAutoria")] DTO_Autor autor)
        {
            var livro = await _context.Livros.FindAsync(LivroId);
            if (livro != null)
            {
                LivroAutor? autoria = new()
                { 
                    LivroId = livro.Id,
                    AutorId = autor.AutorId,
                    OrdemAutoria = autor.OrdemAutoria
                };
                livro.LivroAutors.Add(autoria);
                _context.Update(livro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Autoria adicionada com sucesso.";
            }
            else
            {
                TempData["Fail"] = "Erro ao adicionar a Autoria.";
            }
            return RedirectToAction(nameof(Details), new { Id= LivroId});
        }

        [ActionName("_ApagarAutor")]
        public async Task<IActionResult> DeleteAutor(int livroid, int autorid)
        {
            var livro = await _context.Livros.Include(x=>x.LivroAutors).FirstAsync(x=>x.Id == livroid);
            if (livro != null)
            {
                LivroAutor? livroAutor = livro.LivroAutors.FirstOrDefault(x => x.AutorId == autorid);
                livro.LivroAutors.Remove(livroAutor);
                _context.Update(livro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Autor removido com sucesso.";
            }
            else
            {
                TempData["Fail"] = "Erro ao remover o Autor.";
            }
            return RedirectToAction(nameof(Details), new { Id = livroid });
        }


        [HttpPost, ActionName("_AdicionarCategoria")]
        public async Task<IActionResult> AddCategoria(int LivroId, [Bind("LivroId, CategoriaId")] DTO_Categoria categoria)
        {
            var livro = await _context.Livros.FindAsync(LivroId);
            if (livro != null)
            {
                Categorium? cat = _context.Categoria.FirstOrDefault(x => x.Id == categoria.CategoriaId);
                livro.Categoria.Add(cat);
                _context.Update(livro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Categoria adicionada com sucesso.";
            }
            else
            {
                TempData["Fail"] = "Erro ao adicionar a Categoria.";
            }
            return RedirectToAction(nameof(Details), new { Id= LivroId});
        }

        [ActionName("_ApagarCategoria")]
        public async Task<IActionResult> DeleteCategoria(int livroid, int categoriaid)
        {
            var livro = await _context.Livros.Include(x=>x.Categoria).FirstAsync(x=>x.Id == livroid);
            if (livro != null)
            {
                Categorium? categoria = livro.Categoria.FirstOrDefault(x => x.Id == categoriaid);
                livro.Categoria.Remove(categoria);
                _context.Update(livro);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Categoria removida com sucesso.";
            }
            else
            {
                TempData["Fail"] = "Erro ao remover a Categoria.";
            }
            return RedirectToAction(nameof(Details), new { Id = livroid });
        }
    }
}
