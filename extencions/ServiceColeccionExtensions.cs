using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using MediatR;
using AutoMapper;
using uttt.edu.micro.loggin.aplicacion;
using uttt.edu.micro.loggin.persistencia;

namespace uttt.edu.micro.loggin.api.extensiones
{
    public static class ServiceColeccionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Controladores + FluentValidation
            services.AddControllers()
                .AddFluentValidation(cfg =>
                    cfg.RegisterValidatorsFromAssemblyContaining<Nuevo.EjecutarValidacion>());

            // MongoDB Context
            services.AddSingleton<ContextoLogin>();

            // MediatR
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
