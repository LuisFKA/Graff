using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Graff.Data;
using Graff.Models;

namespace Graff.Controllers
{
    public class LancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lances/Index
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Lance.Include(l => l.Pessoa);
            List<Lance> lances = await applicationDbContext.ToListAsync();
            lances = lances.OrderByDescending(i => i).ToList();
            return View(lances);
        }

        // GET: Lances/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lance = await _context.Lance
                .Include(l => l.Pessoa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lance == null)
            {
                return NotFound();
            }

            return View(lance);
        }

        // GET: Lances/Create
        public IActionResult Create(int Id)
        {
            var pessoas = _context.Pessoa;
            if(pessoas == null || pessoas.Count() == 0)
            {
                return View("Views/Error.cshtml", "Antes de fazer um lance, cadastre pelo menos uma pessoa para fazer esse lance.");
            }

            ViewData["PessoaId"] = new SelectList(_context.Pessoa, "Id", "Id");
            ViewData["PessoaNome"] = new SelectList(_context.Pessoa, "Nome", "Nome");
            ViewData["ProdutoId"] = Id;                    
            Lance lance = new Lance();
            lance.ProdutoId = Id;
            return View(lance);
        }

        // POST: Lances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( IFormCollection keys)
        {
            float valor = float.Parse(keys["Valor"].ToString());
            int produtoId = int.Parse(keys["ProdutoId"].ToString());
            string pessoaNome = keys["PessoaNome"].ToString();

            Lance lance = new Lance();
            lance.Valor = valor;
            lance.ProdutoId = produtoId;

            //Pegando a pessoa que está fazendo o lance
            DbSet<Pessoa> prows = _context.Set<Pessoa>();
            foreach(var p in prows)
            {
                if(p.Nome == pessoaNome)
                {
                    if (p.Idade < 18)
                    {
                        return View("Views/Error.cshtml", "Para fazer um lance em um produto, você precisa ter 18 anos, ou mais.");
                    }

                    lance.PessoaId = p.Id;
                    break;
                }
            }

            //Pegando lances do produto.
            DbSet<Lance> rows = _context.Set<Lance>();
            foreach (var l in rows)
            {
                if (l.ProdutoId == lance.ProdutoId)
                {
                    //O lance da pessoa não bate o maior lance do produto.
                    if(lance.Valor <= l.Valor)
                    {
                        return View("Views/Error.cshtml", "O seu lance precisa ser maior do que o lance atual do item");
                    }
                }

                //Pegando a pessoa que fez esse lance
                l.Pessoa = await _context.Pessoa.FirstOrDefaultAsync(m => m.Id == l.PessoaId);
            }

            if (ModelState.IsValid)
            {
                _context.Add(lance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PessoaId"] = new SelectList(_context.Pessoa, "Id", "Id", lance.PessoaId);
            return View(lance);
        }

        // GET: Lances/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lance = await _context.Lance.FindAsync(id);
            if (lance == null)
            {
                return NotFound();
            }

            ViewData["PessoaId"] = new SelectList(_context.Pessoa, "Id", "Id", lance.PessoaId);
            return View(lance);
        }

        // POST: Lances/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Valor,PessoaId")] Lance lance)
        {
            if (id != lance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanceExists(lance.Id))
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
            ViewData["PessoaId"] = new SelectList(_context.Pessoa, "Id", "Id", lance.PessoaId);
            return View(lance);
        }

        // GET: Lances/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lance = await _context.Lance
                .Include(l => l.Pessoa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lance == null)
            {
                return NotFound();
            }

            return View(lance);
        }

        // POST: Lances/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lance = await _context.Lance.FindAsync(id);
            _context.Lance.Remove(lance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanceExists(int id)
        {
            return _context.Lance.Any(e => e.Id == id);
        }
    }
}
