using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.DataLayer.TransactionManagement
{
    public class TransactionManagementSetup
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
