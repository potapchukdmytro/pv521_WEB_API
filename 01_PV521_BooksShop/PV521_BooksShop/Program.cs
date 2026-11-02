using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Initializer;
using PV521_BooksShop.DAL.Repositories;
using Scalar.AspNetCore;
using FluentValidation;
using PV521_BooksShop.Controllers;
using PV521_BooksShop.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("localDb");
    options.UseNpgsql(connectionString);
});

// Repositories
builder.Services.AddScoped<BookRepostiory>();

builder.Services.AddScoped<
    IValidator<AuthorUploadDto>,
    AuthorUploadValidator>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Seed();

app.Run();
