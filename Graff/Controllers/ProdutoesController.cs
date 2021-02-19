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
    public class ProdutoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produtoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produto.ToListAsync());
        }


        // GET: Produtoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);

            //Pegando os lances desse produto
            DbSet<Lance> rows = _context.Set<Lance>();
            List<Lance> produtoLances = new List<Lance>();            
            foreach(var lance in rows)
            {
                if (lance.ProdutoId == produto.Id)
                {
                    produtoLances.Add(lance);

                    //Pegando a pessoa que fez esse lance
                    lance.Pessoa = await _context.Pessoa.FirstOrDefaultAsync(m => m.Id == lance.PessoaId);
                }           
            }

            produtoLances = produtoLances.OrderByDescending(i => i).ToList();

            produto.Lances = produtoLances;

            //return Content("Is lances null?: " + (produto.Lances == null));
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        //POST: for ShowFilterResults
        //Filters the 
        public async Task<IActionResult> ShowFilterResults(IFormCollection keys)
        {
            var filter = keys["filtro"].ToString();
            var id = int.Parse(keys["Id"].ToString());

            Pessoa Pessoa = null;
            if (!string.IsNullOrEmpty(filter))
            {
                //Pegando a pessoa correspondente do Filtro.
                Pessoa = await _context.Pessoa.FirstOrDefaultAsync(m => m.Nome == filter);
            }

            //Pegando o produto em questão.
            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);

            //Pegando os lances desse produto
            DbSet<Lance> rows = _context.Set<Lance>();
            List<Lance> produtoLances = new List<Lance>();
            foreach (var lance in rows)
            {
                if (lance.ProdutoId == produto.Id)
                {
                    if (Pessoa != null)
                    {
                        if (lance.PessoaId == Pessoa.Id)
                        {
                            //Somente adicionando o lance, se for da pessoa cujo filtro 
                            produtoLances.Add(lance);

                            //Pegando a pessoa que fez esse lance
                            lance.Pessoa = await _context.Pessoa.FirstOrDefaultAsync(m => m.Id == lance.PessoaId);
                        }
                    }
                    else
                    {
                        //Somente adicionando o lance, se for da pessoa cujo filtro 
                        produtoLances.Add(lance);

                        //Pegando a pessoa que fez esse lance
                        lance.Pessoa = await _context.Pessoa.FirstOrDefaultAsync(m => m.Id == lance.PessoaId);
                    }
                }
            }

            produtoLances = produtoLances.OrderByDescending(i => i).ToList();

            produto.Lances = produtoLances;

            //return Content("Is lances null?: " + (produto.Lances == null));
            if (produto == null)
            {
                return NotFound();
            }

            return View("Details", produto);
        }

        // GET: Produtoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Valor")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Valor")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            return View(produto);
        }

        // GET: Produtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}
