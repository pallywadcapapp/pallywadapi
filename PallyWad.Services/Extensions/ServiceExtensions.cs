﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PallyWad.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Define types that need matching
            Type scopedRegistration = typeof(ScopedRegistrationAttribute);
            Type singletonRegistration = typeof(SingletonRegistrationAttribute);
            Type transientRegistration = typeof(TransientRegistrationAttribute);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsDefined(scopedRegistration, false) ||
                p.IsDefined(transientRegistration, false) ||
                p.IsDefined(singletonRegistration, false) && !p.IsInterface)
                //.OrderBy(x=>x.Name)
                .Select(s => new
                {
                    Service = s.GetInterface($"I{s.Name}"),
                    Implementation = s
                })
                .Where(x => x.Service != null);


            foreach (var type in types)
            {
                if (type.Implementation.IsDefined(scopedRegistration, false))
                {
                    services.AddScoped(type.Service, type.Implementation);
                }

                if (type.Implementation.IsDefined(transientRegistration, false))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }

                if (type.Implementation.IsDefined(singletonRegistration, false))
                {
                    services.AddSingleton(type.Service, type.Implementation);
                }
            }

        }
    }
}
