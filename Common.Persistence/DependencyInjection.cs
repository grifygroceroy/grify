using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDal(this IServiceCollection services)
        {
            services.AddSingleton(new DbDetail());
            services.AddTransient<IDal, Dal>();
            return services;
        }
    }
}
