using ASP_111.Data;
using ASP_111.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// "будут спрашивать DateService - выдать объект"
// builder.Services.AddSingleton<DateService>();
// "будут спрашивать IDateService - выдать объект DateService"
builder.Services.AddSingleton<IDateService, DateService>();

builder.Services.AddScoped<TimeService>();
builder.Services.AddTransient<DateTimeService>();

// контекст данных
String? connectionString =   // берем из конфигурации строку подключения
    builder.Configuration.GetConnectionString("PlanetScale");
MySqlConnection connection = new(connectionString);
builder.Services.AddDbContext<DataContext>(
    options =>
        options.UseMySql(
            connection,
            ServerVersion.AutoDetect(connection),   // 8.0.23 --> автоопределение
            serverOptions =>
                serverOptions
                    .MigrationsHistoryTable(
                        tableName: HistoryRepository.DefaultTableName,
                        schema: "asp111")
                    .SchemaBehavior(
                        MySqlSchemaBehavior.Translate,
                        (schema, table) => $"{schema}_{table}")
));


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

/* Д.З. Сверстать форму регистрации нового пользователя
 * Логин
 * Имя 
 * Пароль
 * Повтор пароля
 * Email
 * Аватар - выбор файла
 * [v] Согласие с правилами сайта
 * 
 * Использовать Bootstrap рекомендуется
 */