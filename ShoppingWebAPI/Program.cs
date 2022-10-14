using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShoppingWebAPI.Data;
using ShoppingWebAPI.Repositories;
using ShoppingWebAPI.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopOnlineDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingOnlineConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(
    policy => policy.WithOrigins("https://localhost:7121", "https://localhost:7121")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
