using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ubereats.Models.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ubereats.Models.Authorization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddTransient<IPasswordValidator<IdentityUser>, PasswordValidator>(serv => new PasswordValidator(5));

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
});
builder.Host.UseNLog();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthentication(schema =>
{
    schema.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    schema.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    schema.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(config =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!);
        config.SaveToken = true;
        // faslse только для разработки
        config.RequireHttpsMetadata = false;
        config.TokenValidationParameters = new ()
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"]!,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = builder.Configuration["JWT:ValidAudience"]!,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(key),   
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true
        };

    });

//builder.Services.AddSingleton<IJwtConfiguration, JwtConfiguration>();
// Добавление сервисов в контейнер
//builder.Services.AddTransient<IUserRepository, UserRepository>();
//builder.Services.AddTransient<IJwtConfiguration, JwtConfiguration>();
//builder.Services.AddSingleton<IJwtConfiguration, JwtConfiguration>();


var app = builder.Build();


app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseSession();


app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
//});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();























//app.MapGet("/", (RestaurantContext db) => db.Restaurants.ToList());

// Методы получения данных
// Получение всех данных
//app.MapGet("/api/restaurans", async (RestaurantContext db) => await db.Restaurants.ToListAsync());

////получение данных по id
//app.MapGet("/api/restaurans/{id:int}", async (int id, RestaurantContext db) => {
//    //получение ресторана по id
//    Restaurant? rest = await db.Restaurants.FirstOrDefaultAsync(r => r.ID == id && r.isDeleted == false); ;    

//    //если ресторан не найден, то отправляем статус об ошибке
//    if(rest == null) 
//        return Results.NotFound(new { message = "ресторан не найден"});

//    return Results.Json(rest);
//});

////удаление данных по id
//app.MapDelete("/api/restaurans/{id:int}", async (int id, RestaurantContext db) => {
//    //получение данных по id
//    Restaurant? rest = await db.Restaurants.FirstOrDefaultAsync(r => r.ID == id && r.isDeleted == false);

//    //если ресторан не найден, то отправляем статус об ошибке
//    if (rest == null)
//        return Results.NotFound(new { message = "ресторан не найден" });

//    //если найден обновляем isdeleted = true
//    rest.isDeleted = true;
//    await db.SaveChangesAsync();
//    return Results.Json(rest);

//    //либо если навсегда удалять
//    //db.Restaurants.Remove(rest);
//    //await db.SaveChangesAsync();
//    //return Results.Json(rest);
//});

////добавление нового ресторана
//app.MapPost("/api/restaurans", async (Restaurant rest,RestaurantContext db) => {
//    await db.AddAsync(rest);
//    await db.SaveChangesAsync();
//    return rest;
//});

////обновление данных
//app.MapPut("/api/restaurans", async (Restaurant restData, RestaurantContext db) => {
//    Restaurant? rest = await db.Restaurants.FirstOrDefaultAsync(r => r.ID == restData.ID);

//    //если ресторан не найден, то отправляем статус об ошибке
//    if (rest == null)
//        return Results.NotFound(new { message = "ресторан не найден" });

//    //если найден обновляем данные
//    rest.RestName = restData.RestName;
//    rest.District = restData.District;
//    rest.KitchenType = restData.KitchenType;
//    rest.CookingTime = restData.CookingTime;
//    rest.Image = restData.Image;
//    await db.SaveChangesAsync();
//    return Results.Json(rest);
//});