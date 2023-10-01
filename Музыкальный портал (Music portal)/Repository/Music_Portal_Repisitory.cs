using Microsoft.EntityFrameworkCore;
using Музыкальный_портал__Music_portal_.Models;

namespace Музыкальный_портал__Music_portal_.Repository
{
    public class Music_Portal_Repisitory:IRepository
    {
        private readonly Music_PortalContext _context;

        public Music_Portal_Repisitory(Music_PortalContext context)
        {
            _context = context;
        }
        public async Task<User> GetUser(string login)
        {
            var user =  _context.Users.FirstOrDefault(u => u.Login == login);
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            return user;
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            User? c = await _context.Users.FindAsync(id);
            if (c != null)
                _context.Users.Remove(c);
        }
    }
}
