using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Музыкальный_портал__Music_portal_.Models;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class MusicStyleController : Controller
    {
        private readonly Music_PortalContext _context;

        public MusicStyleController(Music_PortalContext context)
        {
            _context = context;
        }
         
        public async Task<IActionResult> Index()
        {
            IEnumerable<MusicStyle> styles = await Task.Run(() => _context.MusicStyles.ToListAsync());
            ViewBag.MusicStyles = styles;
            return View();
            
        }
         
        public IActionResult Create()
        {
            return View();
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StyleName")] MusicStyle style)
        {
            if (ModelState.IsValid)
            {
                _context.Add(style);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(style);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.MusicStyles.SingleOrDefaultAsync(m => m.Id == id);
            if (style == null)
            {
                return NotFound();
            }
            return View(style);
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StyleName")] MusicStyle style)
        {
            if (id != style.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(style);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicStyleExists(style.Id))
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
            return View(style);
        }

         
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.MusicStyles
                .SingleOrDefaultAsync(m => m.Id == id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var style = await _context.MusicStyles.SingleOrDefaultAsync(m => m.Id == id);
            _context.MusicStyles.Remove(style);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MusicStyleExists(int id)
        {
            return _context.MusicStyles.Any(e => e.Id == id);
        }
    }
}
