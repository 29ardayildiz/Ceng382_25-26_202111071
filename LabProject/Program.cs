using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; 
using LabProject.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SchoolDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));

builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

app.UseStaticFiles(); 
app.UseSession(); 
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();