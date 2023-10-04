using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Музыкальный_портал__Music_portal_.Models;
using Музыкальный_портал__Music_portal_.Repository;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class MusicStyleController : Controller
    {
        IRepository _repository;

        public MusicStyleController(IRepository repository)
        {
            _repository = repository;
        }
         
        public async Task<IActionResult> Index()
        {
            IEnumerable<MusicStyle> styles = await Task.Run(() => _repository.GetMusicStyles());
            ViewBag.MusicStyles = styles;
            return View();
            
        }
         
        public IActionResult Create()
        {
            return PartialView();
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StyleName")] MusicStyle style)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddMusicStyle(style);
                await _repository.Save();
                return PartialView("~/Views/Music/Success.cshtml");
            }
            return PartialView(style);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _repository.GetMusicStyles() == null)
            {
                return NotFound();
            }

            var style = await _repository.GetMusicStyleById((int)id);
            if (style == null)
            {
                return NotFound();
            }
            return PartialView(style);
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
                    _repository.UpdateMusicStyle(style);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MusicStyleExists(style.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return PartialView("~/Views/Music/Success.cshtml");
            }
            return PartialView(style);
        }

         
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _repository.GetMusicStyles() == null)
            {
                return NotFound();
            }

            var style = await _repository.GetMusicStyleById((int)id);
            if (style == null)
            {
                return NotFound();
            }

            return PartialView(style);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _repository.GetMusicStyles() == null)
            {
                return Problem("Entity set 'Music_PortalContext.MusicStyles'  is null.");
            }

            var style = await _repository.GetMusicStyleById(id);
            if (style != null)
            {
                await _repository.DeleteMusicStyle(id);
            }

            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MusicStyleExists(int id)
        {
            List<MusicStyle> list = await _repository.GetMusicStyles();
            return (list?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
