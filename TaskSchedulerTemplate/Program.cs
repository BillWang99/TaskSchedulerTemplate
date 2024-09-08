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


//�]�w�s�u�r��
builder.Services.AddScoped<SqlConnection, SqlConnection>(_ =>
{
    var conn = new SqlConnection();
    conn.ConnectionString = builder.Configuration.GetConnectionString("Connection");
    return conn;
});


//Identity
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option => {
    //���n�J���ɦV
    option.LoginPath = new PathString("/Home/Index");

    //�v�������ɦV
    option.AccessDeniedPath = new PathString("");

    //cookie���Ĵ���(20����)
    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});


//HomeController ���U����
builder.Services.AddRazorPages();
builder.Services.AddFluentValidation();
builder.Services.AddFluentValidationClientsideAdapters();
//���U�������
builder.Services.AddScoped<IValidator<RegisterViewModel>, RegisterValidator>();
//�n�J�������
builder.Services.AddScoped<IValidator<LoginViewModel>, LoginValidator>();

//���U�b��Service
builder.Services.AddScoped<IRegisterService, RegisterService>();
//�n�JService
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
