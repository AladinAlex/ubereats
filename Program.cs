using Microsoft.EntityFrameworkCore;
using ubereats.Models;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantContext>(options => options.UseSqlServer(connection));

//// Add services to the container.
builder.Services.AddControllersWithViews();

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
