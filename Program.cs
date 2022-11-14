global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


var app = builder.Build();

//app.UseSwaggerUI();
//app.UseSwagger(x => x.SerializeAsV2 = true);

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
