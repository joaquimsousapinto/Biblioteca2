using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca2.Models;

namespace Biblioteca2.Controllers
{
    public class EstadosController(Biblioteca2Context context) : Controller
    {
        private readonly Biblioteca2Context _context = context;

        // GET: Estados
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmprestimoEstados
                .Include(x => x.Emprestimos)
                .OrderBy(x => x.Nome).ToListAsync());
        }

        // GET: Estados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimoEstado = await _context.EmprestimoEstados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimoEstado == null)
            {
                return NotFound();
            }

            return View(emprestimoEstado);
        }

        // GET: Estados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] EmprestimoEstado emprestimoEstado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emprestimoEstado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na criação do Registo.";
            return View(emprestimoEstado);
        }

        // GET: Estados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimoEstado = await _context.EmprestimoEstados.FindAsync(id);
            if (emprestimoEstado == null)
            {
                return NotFound();
            }
            return View(emprestimoEstado);
        }

        // POST: Estados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] EmprestimoEstado emprestimoEstado)
        {
            if (id != emprestimoEstado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(emprestimoEstado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo editado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro na edição do Registo.";
            return View(emprestimoEstado);
        }

        // GET: Estados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimoEstado = await _context.EmprestimoEstados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimoEstado == null)
            {
                return NotFound();
            }

            return View(emprestimoEstado);
        }

        // POST: Estados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimoEstado = await _context.EmprestimoEstados.FindAsync(id);
            if (emprestimoEstado != null)
            {
                _context.EmprestimoEstados.Remove(emprestimoEstado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registo apagado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Fail"] = "Erro a apagar o Registo.";
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoEstadoExists(int id)
        {
            return _context.EmprestimoEstados.Any(e => e.Id == id);
        }
    }
}
