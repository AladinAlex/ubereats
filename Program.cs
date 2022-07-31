using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ubereats.Models;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantContext>(options => options.UseSqlServer(connection));

//// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });


var app = builder.Build();


app.UseDefaultFiles();

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

app.Map("/Autorization/{username}", (string username) =>
{
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

    return new JwtSecurityTokenHandler().WriteToken(jwt);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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

app.Run();


public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}