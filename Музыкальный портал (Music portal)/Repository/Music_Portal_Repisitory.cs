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
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
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

        public async Task DeleteUser(int id)
        {
            User? u = await _context.Users.FindAsync(id);
            if (u != null)
                _context.Users.Remove(u);
        }

        //Singer
        public async Task<Singer> GetSingerById(int id)
        {
            var singer = await _context.Singers.SingleOrDefaultAsync(m => m.Id == id);
            return singer;
        }

        public async Task<List<Singer>> GetSingers()
        {
            var singers = await _context.Singers.ToListAsync();
            return singers;
        }

        public async Task AddSinger(Singer singer)
        {
            await _context.Singers.AddAsync(singer);
        }

        public void UpdateSinger(Singer singer)
        {
            _context.Entry(singer).State = EntityState.Modified;
        }

        public async Task DeleteSinger(int id)
        {
            Singer? s = await _context.Singers.FindAsync(id);
            if (s != null)
                _context.Singers.Remove(s);
        }

        //MusicStyle
        public async Task<MusicStyle> GetMusicStyleById(int id)
        {
            var style = await _context.MusicStyles.SingleOrDefaultAsync(m => m.Id == id);
            return style;
        }

        public async Task<List<MusicStyle>> GetMusicStyles()
        {
            var styles = await _context.MusicStyles.ToListAsync();
            return styles;
        }

        public async Task AddMusicStyle(MusicStyle style)
        {
            await _context.MusicStyles.AddAsync(style);
        }

        public void UpdateMusicStyle(MusicStyle style)
        {
            _context.Entry(style).State = EntityState.Modified;
        }

        public async Task DeleteMusicStyle(int id)
        {
            MusicStyle? ms = await _context.MusicStyles.FindAsync(id);
            if (ms != null)
                _context.MusicStyles.Remove(ms);
        }

        //Music
        public async Task<Music> GetMusicById(int id)
        {
            var music = await _context.Musics.SingleOrDefaultAsync(m => m.Id == id);
            return music;
        }

        public async Task<List<Music>> GetMusics()
        {
            var musics =  _context.Musics.Include(u => u.User).Include(u => u.MusicStyle).Include(u => u.Singer);
            return await musics.ToListAsync();
        }

        public async Task AddMusic(Music music)
        {
            await _context.Musics.AddAsync(music);
        }

        public void UpdateMusic(Music music)
        {
            _context.Entry(music).State = EntityState.Modified;
        }

        public async Task DeleteMusic(int id)
        {
            Music? m = await _context.Musics.FindAsync(id);
            if (m != null)
                _context.Musics.Remove(m);
        }


        //All
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
