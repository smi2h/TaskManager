using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.DataLayer.Repositories;
using TaskManager.DataLayer.Tasks;
using TaskManager.DataLayer.TransactionManagement;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.TasksImpl;
using TaskManager.Universal.DateTime.DateTimeProvider;
using Xrm.Pcc.DataLayer.EntityFramework.Repositories;

namespace TaskManager.Api.Rest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors();
            services.AddMvc();
            services.AddTmDbInMemory(Configuration);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepositoryFactory, UnitOfWork>();
            services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));
            services.AddScoped(typeof(IDbTmRepository<>), typeof(DbTmRepository<>));
            services.AddScoped(typeof(IManyToManyRepository<>), typeof(ManyToManyRepository<>));

            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskStateMachine, TaskStateMachine>();

            services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();
            services.AddSingleton<IGuidFactory, GuidFactory>();

            DateTimeGetter.SetProvider(new DefaultDateTimeProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
