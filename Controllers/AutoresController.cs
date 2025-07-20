using Biblioteca2.Models;
using Biblioteca2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.EF;

namespace Biblioteca2.Controllers
{
    public class AutoresController(Biblioteca2Context context, IOptions<AppSettings> appSettings) : Controller
    {
        private readonly Biblioteca2Context _context = context;
        private AppSettings AppSettings { get; } = appSettings.Value;

        // GET: Autores
        public async Task<IActionResult> Index(int? page, string q)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int pageSize = AppSettings.ItensPorPagina;
            int pageNumber = page ?? 1;
            ViewData["q"] = q;
            IPagedList<Autor> retVal;
            if (!string.IsNullOrEmpty(q))
            {
                retVal = await _context.Autors
                    .Where(x => x.Nome.Contains(q))
                    .Include(x => x.LivroAutors).ThenInclude(x => x.Livro)
                    .OrderBy(x => x.Nome)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                retVal = await _context.Autors
                    .Include(x => x.LivroAutors).ThenInclude(x => x.Livro)
                    .OrderBy(x => x.Nome)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            stopwatch.Stop();
            ViewData["ElapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;

            return View(retVal);
        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .Include(a => a.LivroAutors).ThenInclude(la => la.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Bibliografia,DataNascimento,DataMorte")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na criação do Registo.";
            return View(autor);
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Bibliografia,DataNascimento,DataMorte")] Autor autor)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(autor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo editado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na edição do Registo.";
            return View(autor);
        }

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor != null)
            {
                _context.Autors.Remove(autor);
                TempData["Success"] = "Registo apagado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro a apagar o Registo.";
            return RedirectToAction(nameof(Index));
        }
    }
}
