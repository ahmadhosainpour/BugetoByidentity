using Buget_store.Application.Interface.Context;
using Buget_store.Application.Service.User.Queries.GetUsers;
using Bugeto_store.Domain.Entities.User;
using Bugeto_store.Persistence.Context;
using EndPoint.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// ثبت سرویس‌های Entity Framework Core

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("EndPoint"))

    );
builder.Services.AddScoped<IDatabaseContext, DataBaseContext>();
builder.Services.AddScoped<IMessageSender, MessageSender>();

builder.Services.AddScoped<IGetUserService, GetUsersservice>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();//register service of hashing pass
builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
{
    option.Password.RequiredUniqueChars = 0;//default=1
    option.User.RequireUniqueEmail = true;
    option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
    option.Lockout.MaxFailedAccessAttempts = 3;//default=5

}).AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();    


app.UseEndpoints(endpoints =>
{



    endpoints.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

    endpoints.MapControllerRoute(
      name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
