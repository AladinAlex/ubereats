using ubereats;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ubereats.Models;
using ubereats.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantContext>(options => options.UseSqlServer(connection));

builder.Services.AddLogging(logging => {
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
}) ;
builder.Host.UseNLog();

builder.Services.AddAuthentication("AladinAuth").AddJwtBearer("AladinAuth", config => {
    config.TokenValidationParameters = JwtConfiguration.GetTokenValidationParameters();
});


// Добавление сервисов в контейнер
builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseDefaultFiles();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Map("/Authentication/login/{login}&{password}", (string login, string password) => {
    Console.WriteLine("You are in Authentication/login");
    });


//app.Map("/Restaurant/Restaurant/{id}", (int id) => {
//    Console.WriteLine($"Rest's id = {id}");
//});

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