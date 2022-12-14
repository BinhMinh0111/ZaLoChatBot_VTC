global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using System.Configuration;
using ZaloOA_v2.Controllers;
using ZaloOA_v2.Models.DAL.IRepository;
using ZaloOA_v2.Models.DAL.Repositories;
using ZaloOA_v2.Models.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Add DbContext to Repositories.
builder.Services.AddDbContext<db_a8ebff_kenjenorContext>(options =>
{
    options.UseSqlServer
    (
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }
    );
});

//Add Interfaces to Repositories.
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IMessagesRepository,MessagesRepository>();
builder.Services.AddScoped<IPicturesRepository,PicturesRepository>();
builder.Services.AddScoped<INoticesRepository, NoticesRepository>();


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
