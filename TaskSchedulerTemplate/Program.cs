using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using TaskSchedulerTemplate.Migrations;
using Microsoft.AspNetCore.Hosting.Server;
using FluentValidation.AspNetCore;
using FluentValidation;
using TaskSchedulerTemplate.ViewModels.Home;
using TaskSchedulerTemplate.Interface.Home;
using TaskSchedulerTemplate.Service.Home;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//設定連線字串
builder.Services.AddScoped<SqlConnection, SqlConnection>(_ =>
{
    var conn = new SqlConnection();
    conn.ConnectionString = builder.Configuration.GetConnectionString("Connection");
    return conn;
});


//Identity
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option => {
    //未登入的導向
    option.LoginPath = new PathString("/Home/Index");

    //權限不足導向
    option.AccessDeniedPath = new PathString("");

    //cookie有效期限(20分鐘)
    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});


//HomeController 註冊驗證
builder.Services.AddRazorPages();
builder.Services.AddFluentValidation();
builder.Services.AddFluentValidationClientsideAdapters();
//註冊資料驗證
builder.Services.AddScoped<IValidator<RegisterViewModel>, RegisterValidator>();
//登入資料驗證
builder.Services.AddScoped<IValidator<LoginViewModel>, LoginValidator>();

//註冊帳號Service
builder.Services.AddScoped<IRegisterService, RegisterService>();
//登入Service
builder.Services.AddScoped<ILoginSerivce, LoginService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
