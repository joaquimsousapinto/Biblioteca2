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
    public class UtilizadoresController(Biblioteca2Context context, IOptions<AppSettings> appSettings) : Controller
    {
        private readonly Biblioteca2Context _context = context;
        private AppSettings AppSettings { get; } = appSettings.Value;

        // GET: Utilizadores
        public async Task<IActionResult> Index(int? page, string q)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int pageSize = AppSettings.ItensPorPagina;
            int pageNumber = page ?? 1;
            ViewData["q"] = q;
            IPagedList<Utilizador> retVal;
            if (!string.IsNullOrEmpty(q))
            {
                retVal = await _context.Utilizadors
                    .Where(x => x.Nome.Contains(q))
                    .Include(x => x.Emprestimos)
                    .OrderBy(x => x.Nome)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                retVal = await _context.Utilizadors
                    .Include(x => x.Emprestimos)
                    .OrderBy(x => x.Nome)
                    .ToPagedListAsync(pageNumber, pageSize);
            }
            stopwatch.Stop();
            ViewData["ElapsedMilliseconds"] = stopwatch.ElapsedMilliseconds;

            return View(retVal);
        }

        // GET: Utilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // GET: Utilizadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Morada,Telefone,Email,DataNascimento,Sexo")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Morada,Telefone,Email,DataNascimento,Sexo")] Utilizador utilizador)
        {
            if (id != utilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador != null)
            {
                _context.Utilizadors.Remove(utilizador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizadors.Any(e => e.Id == id);
        }
    }
}
