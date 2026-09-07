using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SRSProject.Domain.Contract;
using SRSProject.Infrastructure.DataContext;
using SRSProject.Infrastructure.Repostory;


namespace SRSProject.Infrastructure
{
    public static class InfrastructureRegisterSevice
    {
        public static IServiceCollection InfrastructureRegisterServiceMethod(this IServiceCollection serviceProvider , IConfiguration configuration)
        {
            serviceProvider.AddDbContext<SRSDbContext>(option=>option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")) );
            serviceProvider.AddScoped<IUnitOfWork,UnitOfWork>();
            return serviceProvider;
        }
    }
}
