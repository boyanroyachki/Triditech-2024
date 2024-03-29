﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Web.Triditech.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Automatically adds services to the Inversion of Control Container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }

            Type[] serviceTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach (Type st in serviceTypes)
            {
                Type? interfaceType = st.GetInterface($"I{st.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException($"No interface is provided for the service with name {st.Name}");
                }
                services.AddScoped(interfaceType, st);
            }
        }
    }
}
