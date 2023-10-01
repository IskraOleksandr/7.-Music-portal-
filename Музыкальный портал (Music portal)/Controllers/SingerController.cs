using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Музыкальный_портал__Music_portal_.Models;
using Музыкальный_портал__Music_portal_.Repository;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class SingerController : Controller
    {
        IRepository _repository;

        public SingerController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Singer> singers = await Task.Run(() => _repository.GetSingers());
            ViewBag.Singers = singers;
            return View();
        }

         
        public IActionResult Create()
        {
            return View();
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SingerName")] Singer singer)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddSinger(singer);
                await _repository.Save();
                return RedirectToAction("Index");
            }
            return View(singer);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _repository.GetSingers() == null)
            {
                return NotFound();
            }

            var singer = await _repository.GetSingerById((int)id);
            if (singer == null)
            {
                return NotFound();
            }
            return View(singer);
        }

        
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
                    _repository.UpdateSinger(singer);
                    await _repository.Save();
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

         
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _repository.GetSingers() == null)
            {
                return NotFound();
            }

            var singer = await _repository.GetSingerById((int)id);
            if (singer == null)
            {
                return NotFound();
            }

            return View(singer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _repository.GetSingers() == null)
            {
                return Problem("Entity set 'Music_PortalContext.Singers'  is null.");
            }

            var singer = await _repository.GetSingerById(id);
            if (singer != null)
            {
                await _repository.DeleteSinger(id);
            }

            await _repository.Save();
            return RedirectToAction("Index");
        }

        private async Task<bool> SingerExists(int id)
        {
            List<Singer> list = await _repository.GetSingers();
            return (list?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
