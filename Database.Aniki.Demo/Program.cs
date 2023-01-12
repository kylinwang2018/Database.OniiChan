using Database.Aniki;
using Database.Aniki.Demo;
using Database.Aniki.Demo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSqlServerDbContext<ApplicationDbContext>(
    options =>
    {
        options.ConnectionSting = "Server=(localdb)\\MSSQLLocalDB; Database=NoEntityFrameWorkDemo; MultipleActiveResultSets=true;TrustServerCertificate=true;Min Pool Size=5";
        options.NumberOfTries = 6;
        options.MaxTimeInterval = 5;
        options.DbCommandTimeout = 20;
        options.EnableStatistics = true;
    })
    .RegisterSqlServerRepositories("Database.Aniki.Demo.Repo.MsSql");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
