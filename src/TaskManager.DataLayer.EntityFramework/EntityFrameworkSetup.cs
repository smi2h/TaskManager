using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.DataLayer.Repositories;
using Xrm.Pcc.DataLayer.EntityFramework.Repositories;

namespace Xrm.Pcc.DataLayer.EntityFramework
{
    public class EntityFrameworkSetup
    {
//        public static void Register(IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddDbContext<TmContext>(options => options.UseNpgsql(configuration["ConnectionStrings:PgSqlServer"]));
//            services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));
//            services.AddScoped(typeof(IDbTmRepository<>), typeof(DbTmRepository<>));
//        }
    }
}