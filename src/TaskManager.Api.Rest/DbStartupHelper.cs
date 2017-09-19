using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xrm.Pcc.DataLayer.EntityFramework;

namespace TaskManager.Api.Rest
{
    public static class DbStartupHelper
    {
        public static IServiceCollection AddTmDbInMemory(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddDbContext<TmContext>(opt => opt.UseInMemoryDatabase());
            return services;
        }
    }
}
