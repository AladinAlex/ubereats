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
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
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
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // ����� �������� 2 ������
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

    return new JwtSecurityTokenHandler().WriteToken(jwt);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapGet("/", (RestaurantContext db) => db.Restaurants.ToList());

// ������ ��������� ������
// ��������� ���� ������
//app.MapGet("/api/restaurans", async (RestaurantContext db) => await db.Restaurants.ToListAsync());

////��������� ������ �� id
//app.MapGet("/api/restaurans/{id:int}", async (int id, RestaurantContext db) => {
//    //��������� ��������� �� id
//    Restaurant? rest = await db.Restaurants.FirstOrDefaultAsync(r => r.ID == id && r.isDeleted == false); ;    

//    //���� �������� �� ������, �� ���������� ������ �� ������
//    if(rest == null) 
//        return Results.NotFound(new { message = "�������� �� ������"});

//    return Results.Json(rest);
//});

////�������� ������ �� id
//app.MapDelete("/api/restaurans/{id:int}", async (int id, RestaurantContext db) => {
//    //��������� ������ �� id
//    Restaurant? rest = await db.Restaurants.FirstOrDefaultAsync(r => r.ID == id && r.isDeleted == false);

//    //���� �������� �� ������, �� ���������� ������ �� ������
//    if (rest == null)
//        return Results.NotFound(new { message = "�������� �� ������" });

//    //���� ������ ��������� isdeleted = true
//    rest.isDeleted = true;
//    await db.SaveChangesAsync();
//    return Results.Json(rest);

//    //���� ���� �������� �������
//    //db.Restaurants.Remove(rest);
//    //await db.SaveChangesAsync();
//    //return Results.Json(rest);
//});

////���������� ������ ���������
//app.MapPost("/api/restaurans", async (Restaurant rest,RestaurantContext db) => {
//    await db.AddAsync(rest);
//    await db.SaveChangesAsync();
//    return rest;
//});

////���������� ������
//app.MapPut("/api/restaurans", async (Restaurant restData, RestaurantContext db) => {
//    Restaurant? rest = await db.Restaurants.FirstOrDefaultAsync(r => r.ID == restData.ID);

//    //���� �������� �� ������, �� ���������� ������ �� ������
//    if (rest == null)
//        return Results.NotFound(new { message = "�������� �� ������" });

//    //���� ������ ��������� ������
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
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}