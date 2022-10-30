using Database.Aniki;
using Database.Aniki.Demo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbOption>(options =>
    {
        options.ConnectionSting = "Server=192.168.1.26\\SQl2019; Database=CloudNativeCopy; UID=demo; PWD=DemoPassword; MultipleActiveResultSets=true;TrustServerCertificate=true";
        options.NumberOfTries = 6;
        options.MaxTimeInterval = 5;
        options.DbCommandTimeout = 20;
        options.EnableStatistics = true;
    }).UseSqlServer();

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
