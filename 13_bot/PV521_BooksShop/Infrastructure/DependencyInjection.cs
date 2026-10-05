using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.DAL.Abstraction;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;
using Quartz;
using System.Text;

namespace PV521_BooksShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<BookService>();
            services.AddScoped<ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserService>();
            services.AddScoped<PasswordHasher<User>>();
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<EmailService>();
            services.AddScoped<RoleService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<BookRepostiory>();
            services.AddScoped<AuthorRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<UserTokenRepository>();
            services.AddScoped<RoleRepository>();

            return services;
        }

        public static IServiceCollection AddJobs(this IServiceCollection services, params (Type type, string cronExpression)[] jobs)
        {
            services.AddQuartz(q =>
            {
                foreach (var job in jobs)
                {
                    var jobKey = new JobKey(job.type.Name);
                    q.AddJob(job.type, configure: cfg => cfg.WithIdentity(jobKey));

                    q.AddTrigger(opt => opt
                        .ForJob(jobKey)
                        .WithIdentity($"{job.type.Name}-trigger")
                        .WithCronSchedule(job.cronExpression));
                }
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]
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

            return services;
        }
    }
}
