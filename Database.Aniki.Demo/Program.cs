using Database.Aniki;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext(options =>
{
    options.ConnectionSting = "Server=192.168.1.26\\SQl2019; Database=CloudNativeCopy; UID=demo; PWD=DemoPassword; MultipleActiveResultSets=true;TrustServerCertificate=true";
    options.RetryWaitingSeconds = 6;
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