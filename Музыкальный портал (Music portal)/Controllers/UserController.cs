using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Музыкальный_портал__Music_portal_.Models;
using Музыкальный_портал__Music_portal_.Repository;

namespace Музыкальный_портал__Music_portal_.Controllers
{
    public class UserController : Controller
    {
        IRepository _repository;

        public UserController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        { 
            return View(await _repository.GetUsers());
        }
         
        public ActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login_Model logon)
        {
            if (ModelState.IsValid)
            {
                var users = await _repository.GetUsers();
                if (users.Count == 0)
                {
                    ModelState.AddModelError("", "Не коректный логин или пароль!");
                    return PartialView(logon);
                }
                var users_t = users.Where(a => a.Login == logon.Login);
                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Не коректный логин или пароль!");
                    return PartialView(logon);
                }
                var user = users_t.First();
                string? salt = user.Salt;

                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);//переводим пароль в байт-массив  

                var md5 = MD5.Create();//создаем объект для получения средств шифрования  

                byte[] byteHash = md5.ComputeHash(password);//вычисляем хеш-представление в байтах  

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Не коректный логин или пароль!");
                    return PartialView(logon);
                }
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetInt32("Level", user.Level);
                return PartialView("~/Views/Music/Success.cshtml");
            }
            return PartialView(logon);
        }

        public IActionResult Register()
        {
            return PartialView("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register_Model reg)
        {
            if (reg.Login == "admin")
                ModelState.AddModelError("Login", "admin - запрещенный логин");
             
            if (ModelState.IsValid)
            {
                User user = new User();
                user.FirstName = reg.FirstName; 
                user.LastName = reg.LastName;
                user.Login = reg.Login;
                user.Email = reg.Email;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);//переводим пароль в байт-массив  

                var md5 = MD5.Create();//создаем объект для получения средств шифрования  

                byte[] byteHash = md5.ComputeHash(password);//вычисляем хеш-представление в байтах  

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();
                user.Salt = salt;
                user.Level = reg.Level;

                await _repository.AddUser(user);
                await _repository.Save();
                return PartialView("~/Views/Music/Success.cshtml");
            }

            return PartialView("Register");
        }
         
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _repository.GetUsers() == null)
            {
                return NotFound();
            }

            var user = await _repository.GetUserById((int)id);
            if (user == null)
            {
                return NotFound();
            }
            return PartialView(user);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Login,Email,Password,Salt,Level")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                { 
                     _repository.UpdateUser(user);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(user.Id))
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
            return PartialView(user);
        }
         
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _repository.GetUsers() == null)
            {
                return NotFound();
            }

            var user = await _repository.GetUserById((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return PartialView(user);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _repository.GetUsers() == null)
            {
                return Problem("Entity set 'Music_PortalContext.Students'  is null.");
            }

            var user = await _repository.GetUserById(id);
            if (user != null)
            {
                await _repository.DeleteUser(id);
            }

            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExists(int id)
        {
            List<User> list = await _repository.GetUsers();
            return (list?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            if (email == "admin@mail.ru" || email == "admin@gmail.com")
                return Json(false);
            return Json(true);
        }
    }
}
