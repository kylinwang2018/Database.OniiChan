using Database.Aniki.Demo.Models;
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

        public async Task OnGetAsync()
        {
            var result = await _dbContext.GetListOfAsync<User>("select * from ir_empl", CommandType.Text);
            Console.WriteLine(result);
        }
    }
}