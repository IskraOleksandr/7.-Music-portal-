using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Музыкальный_портал__Music_portal_.Models;
using Музыкальный_портал__Music_portal_.Repository;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class MusicController : Controller
    {
        IRepository _repository; 
        private readonly IWebHostEnvironment _appEnvironment;

        public MusicController(IRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _appEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Music> singers = await Task.Run(() => _repository.GetMusics());
            ViewBag.Musics = singers;
            return View("Index");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Music");
        } 

        public async Task<IActionResult> Create()
        {
            var styles = await _repository.GetMusicStyles();
            var singers = await _repository.GetSingers();

            ViewBag.Style_List = new SelectList(styles, "Id", "StyleName");
            ViewBag.Singer_List = new SelectList(singers, "Id", "SingerName");
            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Video_Name,Album,Year,Video_URL,VideoDate,MusicStyleId,SingerId,UserId")] Music music, IFormFile Video_URL)
        {
            
            var user_login = HttpContext.Session.GetString("Login");

                if (HttpContext.Session.GetString("Login") == null)
                    return PartialView("~/Views/User/Login.cshtml");


                var us = await _repository.GetUser(user_login);
            music.User = us;

            var style = await _repository.GetMusicStyleById(music.MusicStyleId);
            music.MusicStyle = style;

            var singer = await _repository.GetSingerById(music.SingerId);
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
                else
                {
                    ModelState.AddModelError("", "Выберите файл песни!");
                    return PartialView();
                }

                await _repository.AddMusic(music);
                await _repository.Save(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MusicExists(music.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return PartialView("Create");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _repository.GetMusics() == null)
            {
                return NotFound();
            }

            var music = await _repository.GetMusicById((int)id);
            if (music == null)
            {
                return NotFound();
            }

            var styles = await _repository.GetMusicStyles();
            var singers = await _repository.GetSingers();

            ViewBag.Style_List = new SelectList(styles, "Id", "StyleName");
            ViewBag.Singer_List = new SelectList(singers, "Id", "SingerName");
            return PartialView(music);
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
                    return PartialView("~/Views/User/Login.cshtml");

                var user_login = HttpContext.Session.GetString("Login");

                var style = await _repository.GetMusicStyleById(music.MusicStyleId);
                var singer = await _repository.GetSingerById(music.SingerId);
                var us = await _repository.GetUser(user_login);

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
                    return PartialView();
                }

                 _repository.UpdateMusic(music);
                await _repository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MusicExists(music.Id))
                {
                    return NotFound();
                }
                else throw;
            }
            return PartialView("~/Views/Music/Success.cshtml"); 
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _repository.GetMusics() == null)
            {
                return NotFound();
            }

            var music = await _repository.GetMusicById((int)id);
            if (music == null)
            {
                return NotFound();
            }

            return PartialView(music);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _repository.GetMusics() == null)
            {
                return Problem("Entity set 'Music_PortalContext.Musics'  is null.");
            }

            var music = await _repository.GetMusicById(id);
            if (music != null)
            {
                await _repository.DeleteMusic(id);
            }

            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MusicExists(int id)
        {
            List<Music> list = await _repository.GetMusics();
            return (list?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}