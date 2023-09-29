using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Музыкальный_портал__Music_portal_.Models;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class MusicController : Controller
    {
        private readonly Music_PortalContext _context;

        private readonly IWebHostEnvironment _appEnvironment;

        public MusicController(Music_PortalContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Music> singers = await Task.Run(() => _context.Musics.Include(u => u.User).Include(u => u.MusicStyle).Include(u => u.Singer));
            ViewBag.Musics = singers;
            return View("Index");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Music");
        }


        public IActionResult Create()
        {
            var styles = _context.MusicStyles.ToList();
            var singers = _context.Singers.ToList();

            ViewBag.Style_List = new SelectList(styles, "Id", "StyleName");
            ViewBag.Singer_List = new SelectList(singers, "Id", "SingerName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Video_Name,Album,Year,Video_URL,VideoDate,MusicStyleId,SingerId,UserId")] Music music, IFormFile Video_URL)
        {
            var user_login = HttpContext.Session.GetString("Login");

            if (HttpContext.Session.GetString("Login") == null)
                return RedirectToAction("Login", "User");


            var us = await _context.Users.SingleOrDefaultAsync(u => u.Login == user_login);
            music.User = us;

            var style = await _context.MusicStyles.SingleOrDefaultAsync(u => u.Id == music.MusicStyleId);
            music.MusicStyle = style;

            var singer = await _context.Singers.SingleOrDefaultAsync(u => u.Id == music.SingerId);
            music.Singer = singer;

            music.UserId = us.Id;
            music.VideoDate = DateTime.Now;
            try
            {
                if (Video_URL != null)
                {
                    string file_path = "/Music/" + Video_URL.FileName;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + file_path, FileMode.Create))
                    {
                        await Video_URL.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    music.Video_URL = "~" + file_path;
                }


                _context.Add(music);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(music.Id))
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }

            var styles = _context.MusicStyles.ToList();
            var singers = _context.Singers.ToList();

            ViewBag.Style_List = new SelectList(styles, "Id", "StyleName");
            ViewBag.Singer_List = new SelectList(singers, "Id", "SingerName");
            return View(music);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Video_Name,Album,Year,Video_URL,VideoDate,MusicStyleId,SingerId,UserId")] Music music, IFormFile Video_URL)
        {
            if (id != music.Id)
                return NotFound();

            try
            {
                if (HttpContext.Session.GetString("Login") == null)
                    return RedirectToAction("Login", "User");

                var user_login = HttpContext.Session.GetString("Login");

                var style = await _context.MusicStyles.SingleOrDefaultAsync(u => u.Id == music.MusicStyleId);
                var singer = await _context.Singers.SingleOrDefaultAsync(u => u.Id == music.SingerId);
                var us = await _context.Users.SingleOrDefaultAsync(u => u.Login == user_login);

                music.MusicStyle = style;
                music.Singer = singer;
                music.User = us;

                music.UserId = us.Id;
                music.VideoDate = DateTime.Now;

                if (Video_URL != null)
                {
                    string file_path = "/Music/" + Video_URL.FileName;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + file_path, FileMode.Create))
                    {
                        await Video_URL.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    music.Video_URL = "~" + file_path;
                }
                else
                {
                    ModelState.AddModelError("", "Выберите файл для изменения песни!");
                    return View();//
                }

                _context.Update(music);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(music.Id))
                {
                    return NotFound();
                }
                else throw;
            } 
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var music = await _context.Musics
                .SingleOrDefaultAsync(m => m.Id == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var music = await _context.Musics.SingleOrDefaultAsync(m => m.Id == id);
            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FilmExists(int id)
        {
            return (_context.Musics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}