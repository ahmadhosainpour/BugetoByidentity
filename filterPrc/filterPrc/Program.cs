
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.HttpLogging;
using Serilog.Debugging;
using System.Diagnostics;
using filterPRc.Models;
using Microsoft.AspNetCore.Mvc.Filters;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<LogActionFilter>();
builder.Host.UseSerilog();
// Enable internal Serilog diagnostics
SelfLog.Enable(msg => Console.Error.WriteLine(msg));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose() // or Debug, Information, depending on your needs
    .WriteTo.Console()     // Log to console
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // Log to file
    .WriteTo.Debug()       // Debug output window
         // Enable internal Serilog diagnostics
    .CreateLogger();

builder.Host.UseSerilog((ctx, lc,LoggerConfiguration) => LoggerConfiguration.ReadFrom.Configuration(ctx.Configuration));
//builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
//// Register ObjectPool<HttpLoggingInterceptorContext>
//builder.Services.AddSingleton<ObjectPool<HttpLoggingInterceptorContext>>(serviceProvider =>
//{
//    var poolProvider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
//    return poolProvider.Create<HttpLoggingInterceptorContext>();
//});
builder.Services.AddSingleton<LogActionFilter>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
});




var app = builder.Build();
//app.UseHttpLogging();
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    var stopwatch = Stopwatch.StartNew();

    // درخواست
    var method = context.Request.Method;
    var path = context.Request.Path;

    await next(); // ادامه پردازش درخواست

    // پاسخ
    var statusCode = context.Response.StatusCode;
    stopwatch.Stop();

    var responseTimeMs = stopwatch.Elapsed.TotalMilliseconds;

    // ثبت لاگ ساخت‌یافته
    logger.LogInformation(
        "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms",
        method, path, statusCode, responseTimeMs);
});
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();