using Microsoft.Extensions.DependencyInjection;
using SRSProject.Application.Contracts;
using SRSProject.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application
{
    public static class ApplicationServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeServices>();
       

            return services;
        }
    }
}
