using Database.Aniki.Demo.Models;
using Database.Aniki.SqlServer;
using System.Data;
using Microsoft.Data.SqlClient;

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
            for (int i = 0; i < 20; i++)
            {
                await using var dr = await _dbContext.ExecuteReaderAsync(
                    "Select * from [dbo].[User]",
                    CommandType.Text
                );
                if (await dr.ReadAsync())
                {
                    Console.WriteLine(dr[1]);
                }
                
            }
            return null;
        }
    }
}
