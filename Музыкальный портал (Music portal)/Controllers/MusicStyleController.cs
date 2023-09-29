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

        // GET: Students
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.MusicStyles.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            IEnumerable<MusicStyle> styles = await Task.Run(() => _context.MusicStyles.ToListAsync());
            ViewBag.MusicStyles = styles;
            return View();
            //return _context.MusicStyles != null ?
            //            View(await ) :
            //            Problem("Entity set 'FilmContext.Films'  is null.");
        }
        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
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

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.MusicStyles.SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StyleName")] MusicStyle student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicStyleExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.MusicStyles
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
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
