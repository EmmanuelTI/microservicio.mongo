using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Options;
using uttt.edu.micro.loggin.aplicacion;
using uttt.edu.micro.loggin.config;      
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.api.extensiones
{
    public static class ServiceColeccionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

         
            services.AddControllers()
                .AddFluentValidation(cfg =>
                    cfg.RegisterValidatorsFromAssemblyContaining<Nuevo.EjecutarValidacion>());

          
            services.AddSingleton<ContextoLogin>();

      
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

        
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
