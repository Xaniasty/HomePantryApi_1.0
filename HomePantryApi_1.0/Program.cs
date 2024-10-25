using Microsoft.EntityFrameworkCore;
using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using HomePantryApi_1._0.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; });

builder.Services.AddControllers();

builder.Services.AddDbContext<SpizDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 27)))); 

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGranaryRepository, GranaryRepository>();
builder.Services.AddScoped<IShoplistRepository, ShoplistRepository>();
builder.Services.AddScoped<IProductsInGranaryRepository, ProductsInGranaryRepository>();
builder.Services.AddScoped<IProductsInShoplistRepository, ProductsInShoplistRepository>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
