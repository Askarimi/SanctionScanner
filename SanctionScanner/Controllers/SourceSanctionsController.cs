using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanctionScanner.Models;

namespace SanctionScanner.Controllers
{
    public class SourceSanctionsController : Controller
    {
        private readonly SanctionScannerDbContext _context;

        public SourceSanctionsController(SanctionScannerDbContext context)
        {
            _context = context;
        }

        // GET: SourceSanctions
        public async Task<IActionResult> Index()
        {
            return View(await _context.SourceSanctions.ToListAsync());
        }

        // GET: SourceSanctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceSanction = await _context.SourceSanctions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sourceSanction == null)
            {
                return NotFound();
            }

            return View(sourceSanction);
        }

        // GET: SourceSanctions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SourceSanctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SourceName,SourceCode,NameFile,FormatFile,HasFile,Id")] SourceSanction sourceSanction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sourceSanction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sourceSanction);
        }

        // GET: SourceSanctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceSanction = await _context.SourceSanctions.FindAsync(id);
            if (sourceSanction == null)
            {
                return NotFound();
            }
            return View(sourceSanction);
        }

        // POST: SourceSanctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SourceName,SourceCode,NameFile,FormatFile,HasFile,Id")] SourceSanction sourceSanction)
        {
            if (id != sourceSanction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sourceSanction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SourceSanctionExists(sourceSanction.Id))
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
            return View(sourceSanction);
        }

        // GET: SourceSanctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceSanction = await _context.SourceSanctions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sourceSanction == null)
            {
                return NotFound();
            }

            return View(sourceSanction);
        }

        // POST: SourceSanctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sourceSanction = await _context.SourceSanctions.FindAsync(id);
            _context.SourceSanctions.Remove(sourceSanction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SourceSanctionExists(int id)
        {
            return _context.SourceSanctions.Any(e => e.Id == id);
        }
    }
}
