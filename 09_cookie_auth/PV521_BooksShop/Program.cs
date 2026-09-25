using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.BLL.Settings;
using PV521_BooksShop.BLL.Validators.Book;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Abstraction;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Initializer;
using PV521_BooksShop.DAL.Repositories;
using PV521_BooksShop.Infrastructure;
using Scalar.AspNetCore;
using System.Runtime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Authorization
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] 
                ?? throw new ArgumentNullException("Jwt token not found"))),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["accessToken"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Add dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("localDb");
    options.UseNpgsql(connectionString);
});

// Add automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODE3OTQyNDAwIiwiaWF0IjoiMTc4NjQ0NjkxOCIsImFjY291bnRfaWQiOiIwMTk5NTEzZTdlYmY3YjYwOGI4Y2I3NTI3YTE3ZTI5MyIsImN1c3RvbWVyX2lkIjoiMDE5OTUxM2U3ZWJmN2I2MDhiOGNiNzUyN2ExN2UyOTMiLCJzdWJfaWQiOiItIiwiZWRpdGlvbiI6IjAiLCJ0eXBlIjoiMiJ9.gnQYP7aLCcVQ_aS_g36BR2TVz1srfcCr3P5xrAw-1S6MNPECaqNweRUZCwbe6OKG6QL64wtDIYoFmuchoaQSmtAXDRldrVvsOcF84i5690kssWPhWRHmrxtas8Tjougl3Cfn64I18iQWfBJtgzAfqhKXVkD1mIc6TwHWrG40LWFpqSQEEZvPa9v3a05p6LIDvuex0ISIY_TFJ0iKVCr17jEWJicLfvoBGbCfEhImV0NeWhGwMQu8Vt5CfY85uuEkXf1Eit9UO8MdD_SlnSUuzXk549mD8w9IJWzjESa-ozntv39zVyUQxDhjHb1qXXn-wS4ALUaOU6NgG8NDbK2Ajw";
}, AppDomain.CurrentDomain.GetAssemblies());

// Add repositories
builder.Services.AddScoped<BookRepostiory>();
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserTokenRepository>();

// Add services
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<EmailService>();

// Add settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Disable default validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();

// Add CORS
const string corsName = "allowFront";
string? allowedOrigin = builder.Configuration["AllowedOrigin"];
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(corsName, cfg =>
    {
        cfg.WithOrigins(allowedOrigin ?? "")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference();
//}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// CORS
app.UseCors(corsName);

app.UseAuthentication();
app.UseAuthorization();

// Static files
app.AddStaticFiles(app.Environment);

app.MapControllers();

app.Seed();

app.Run();
