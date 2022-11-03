using Database.Aniki.Demo.Models;

namespace Database.Aniki.Repo.MsSql
{
    public interface IRepo
    {
        Task<List<User>> GetUsersAsync();
    }
}