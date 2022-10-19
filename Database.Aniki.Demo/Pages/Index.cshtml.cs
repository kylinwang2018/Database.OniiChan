using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Database.Aniki.Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISqlServerDbContext _dbContext;

        public IndexModel(
            ILogger<IndexModel> logger, ISqlServerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            using var sqlCommand = new SqlCommand("select * from Ir_Empl");
            var result = _dbContext.GetColumnToString(sqlCommand, "Username");
            Console.WriteLine(result);
        }
    }
}