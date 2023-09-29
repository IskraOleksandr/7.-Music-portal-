using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Музыкальный_портал__Music_portal_.Models;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class SingerController : Controller
    {
        private readonly Music_PortalContext _context;

        public SingerController(Music_PortalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Singer> singers = await Task.Run(() => _context.Singers.ToListAsync());
            ViewBag.Singers = singers;
            return View();
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SingerName")] Singer singer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(singer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(singer);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers.SingleOrDefaultAsync(m => m.Id == id);
            if (singer == null)
            {
                return NotFound();
            }
            return View(singer);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SingerName")] Singer singer)
        {
            if (id != singer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(singer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SingerExists(singer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(singer);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (singer == null)
            {
                return NotFound();
            }

            return View(singer);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var singer = await _context.Singers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Singers.Remove(singer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SingerExists(int id)
        {
            return _context.Singers.Any(e => e.Id == id);
        }
    }
}
