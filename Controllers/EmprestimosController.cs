using Biblioteca2.Models;
using Biblioteca2.Models.Common;
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
    public class EmprestimosController(Biblioteca2Context context, IOptions<AppSettings> appSettings) : Controller
    {
        private readonly Biblioteca2Context _context = context;
        private AppSettings AppSettings { get; } = appSettings.Value;

        // GET: Emprestimos
        public async Task<IActionResult> Index(int? page, string q)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int pageSize = AppSettings.ItensPorPagina;
            int pageNumber = page ?? 1;
            ViewData["q"] = q;
            IPagedList<Emprestimo> retVal;
            if (!string.IsNullOrEmpty(q))
            {
                retVal = await _context.Emprestimos
                    .Where(x => x.Id.ToString().Equals(q))
                    .Include(x => x.EmprestimoLivros)
                    .Include(x => x.Utilizador)
                    .OrderBy(x => x.Id)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                retVal = await _context.Emprestimos
                    .Include(x => x.EmprestimoLivros)
                    .Include(x => x.Utilizador)
                    .OrderBy(x => x.Id)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            stopwatch.Stop();
            ViewData["ElapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;

            return View(retVal);
        }

        // GET: Emprestimos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Estado)
                .Include(e => e.Utilizador)
                .Include(e => e.EmprestimoLivros).ThenInclude(el => el.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            List<int> tmp = [.. emprestimo.EmprestimoLivros.Select(el => el.LivroId)];
            ViewData["Livros"] = new SelectList(_context.Livros.Where(x => !tmp.Contains(x.Id)).OrderBy(x => x.Titulo).ToList(), "Id", "Titulo");
            return View(emprestimo);
        }

        // GET: Emprestimos/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.EmprestimoEstados, "Id", "Nome");
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Nome");
            return View();
        }

        // POST: Emprestimos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UtilizadorId,EstadoId,DataEmprestimo,DataDevolucaoPrevista,DataDevolucaoReal")] Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emprestimo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na criação do Registo.";
            ViewData["EstadoId"] = new SelectList(_context.EmprestimoEstados, "Id", "Nome", emprestimo.EstadoId);
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Nome", emprestimo.UtilizadorId);
            return View(emprestimo);
        }

        // GET: Emprestimos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.EmprestimoEstados, "Id", "Nome", emprestimo.EstadoId);
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Nome", emprestimo.UtilizadorId);
            return View(emprestimo);
        }

        // POST: Emprestimos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UtilizadorId,EstadoId,DataEmprestimo,DataDevolucaoPrevista,DataDevolucaoReal")] Emprestimo emprestimo)
        {
            if (id != emprestimo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(emprestimo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo editado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na edição do Registo.";
            ViewData["EstadoId"] = new SelectList(_context.EmprestimoEstados, "Id", "Nome", emprestimo.EstadoId);
            ViewData["UtilizadorId"] = new SelectList(_context.Utilizadors, "Id", "Nome", emprestimo.UtilizadorId);
            return View(emprestimo);
        }

        // GET: Emprestimos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Estado)
                .Include(e => e.Utilizador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
                TempData["Success"] = "Registo apagado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro a apagar o Registo.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("_AdicionarLivro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarLivro(int EmprestimoId, [Bind("EmprestimoId,LivroId,Quantidade")] EmprestimoLivro emprestimoLivro)
        {
            var emprestimo = await _context.Emprestimos
                .Include(e => e.EmprestimoLivros)
                .FirstOrDefaultAsync(x => x.Id == EmprestimoId);
            if (emprestimo != null)
            {
                emprestimo.EmprestimoLivros.Add(emprestimoLivro);
                _context.Update(emprestimo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Livro adicionado com sucesso.";
            }
            else
            {
                TempData["Fail"] = "Erro ao adicionar a Livro.";
            }
            return RedirectToAction(nameof(Details), new {Id = EmprestimoId });
        }

        // POST: Emprestimos/Delete/5
        [ActionName("_ApagarLivro")]
        public async Task<IActionResult> ApagarLivro(int EmprestimoId, int LivroId)
        {
            var emprestimo = await _context.Emprestimos
                .Include(e => e.EmprestimoLivros).ThenInclude(el => el.Livro)
                .FirstOrDefaultAsync(x => x.Id == EmprestimoId);
            if (emprestimo != null)
            {
                var livro = emprestimo.EmprestimoLivros.FirstOrDefault(x => x.LivroId == LivroId);
                emprestimo.EmprestimoLivros.Remove(livro);
                _context.Update(emprestimo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Livro removido com sucesso.";
            }
            else
            {
                TempData["Fail"] = "Erro ao remover o Livro.";
            }
            return RedirectToAction(nameof(Details), new { Id = EmprestimoId });
        }
    }
}
