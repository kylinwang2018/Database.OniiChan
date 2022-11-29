using Database.Aniki.Demo.Models;
using Database.Aniki.SqlServer;
using System.Data;

namespace Database.Aniki.Repo.MsSql
{
    [SqlServerRepo]
    internal class Repo : IRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public Repo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            using var dr = await _dbContext.ExecuteReaderAsync(
            "spVSOC_ResetAllQueue",
            CommandType.StoredProcedure
            );
            if (await dr.ReadAsync())
            {
                string output = Convert.ToString(dr["output"]);
            }

            return null;
        }
    }
}
