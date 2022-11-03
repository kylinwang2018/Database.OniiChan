using Database.Aniki.Repo.MsSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Database.Aniki.Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepo _repo;

        public IndexModel(
            ILogger<IndexModel> logger, IRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            var result = await _repo.GetUsersAsync();
            Console.WriteLine(result);
        }
    }
}