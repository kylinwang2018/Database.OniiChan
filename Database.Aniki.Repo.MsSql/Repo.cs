using Database.Aniki.Demo.Models;
using Database.Aniki.SqlServer;
using System.Data;

namespace Database.Aniki.Repo.MsSql
{
    [SqlServerRepo]
    internal class Repo : IRepo
    {
        private readonly ISqlServerDbContext<ApplicationDbOption> _dbContext;

        public Repo(ISqlServerDbContext<ApplicationDbOption> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _dbContext.GetListOfAsync<User>("select * from ir_empl", CommandType.Text);
        }
    }
}
