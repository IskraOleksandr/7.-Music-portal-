using Музыкальный_портал__Music_portal_.Models;

namespace Музыкальный_портал__Music_portal_.Repository
{
    public interface IRepository
    {
        //User
        Task<User> GetUser(string login);
        Task<User> GetUserById(int id);
        Task<List<User>> GetUsers();
        Task AddUser(User user);
        void UpdateUser(User user);
        Task DeleteUser(int id);

        //Singer
        Task<Singer> GetSingerById(int id);
        Task<List<Singer>> GetSingers();
        Task AddSinger(Singer singer);
        void UpdateSinger(Singer singer);
        Task DeleteSinger(int id);

        //MusicStyle
        Task<MusicStyle> GetMusicStyleById(int id);
        Task<List<MusicStyle>> GetMusicStyles();
        Task AddMusicStyle(MusicStyle style);
        void UpdateMusicStyle(MusicStyle style);
        Task DeleteMusicStyle(int id);

        //All
        Task Save();
    }
}
