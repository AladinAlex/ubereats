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
        // faslse ������ ��� ����������
        config.RequireHttpsMetadata = false;
        config.TokenValidationParameters = new ()
        {
            // ��������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"]!,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = builder.Configuration["JWT:ValidAudience"]!,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = new SymmetricSecurityKey(key),   
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true
        };

    });

//builder.Services.AddSingleton<IJwtConfiguration, JwtConfiguration>();
// ���������� �������� � ���������
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