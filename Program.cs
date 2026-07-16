using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Profiles;
using ProductService.Repositories;
using ProductService.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IValidator<ProductRequestDto>, ProductRequestDtoValidator>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ProductProfile>();
}, typeof(ProductProfile).Assembly);

var connectionString=builder.Configuration.GetConnectionString("ProductDB");
builder.Services.AddDbContext<ApplicationDbContext>(options=>{
    options.UseSqlServer(connectionString);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
