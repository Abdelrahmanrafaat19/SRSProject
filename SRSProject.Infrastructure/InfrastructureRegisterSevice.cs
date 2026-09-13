using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SRSProject.Application.Contracts;
using SRSProject.Domain.Contract;
using SRSProject.Infrastructure.DataContext;
using SRSProject.Infrastructure.DataContext.IDentityEntity;
using SRSProject.Infrastructure.Jwt;
using SRSProject.Infrastructure.Repository;
using SRSProject.Infrastructure.Repostory;
using System.Text;


namespace SRSProject.Infrastructure
{
    public static class InfrastructureRegisterSevice
    {
        public static IServiceCollection InfrastructureRegisterServiceMethod(this IServiceCollection serviceProvider, IConfiguration configuration)
        {
            serviceProvider.AddDbContext<SRSDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            serviceProvider.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Stores.SchemaVersion =
                    IdentitySchemaVersions.Version3;
            }).AddEntityFrameworkStores<SRSDbContext>()
            .AddDefaultTokenProviders();
            serviceProvider.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceProvider.AddScoped<IIdentityService, IdentityRepo>();
            serviceProvider.AddScoped<IUserService, UserService>();
            serviceProvider.AddScoped<IJwtCreator, JwtCreator>();
            var jwtSettings = configuration
                                             .GetSection("JwtSettings")
                                             .Get<JwtSettings>()
                                             ?? throw new InvalidOperationException("JWT Settings are missing.");
            serviceProvider
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            return serviceProvider;
        }
    }
}
