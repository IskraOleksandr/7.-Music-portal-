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
        Task Delete(int id);

        //All
        Task Save();
    }
}
