using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.HostHelper
{
    public static class WebHostFactory
    {
        public static IWebHost Create<TStartup>(string defaultBinding, string[] args) where TStartup : class
        {
            return Create<TStartup>(defaultBinding, args, _ => { });
        }

        public static IWebHost Create<TStartup>(string defaultBinding, AppStartupConfig startupConfig) where TStartup : class
        {
            return Create<TStartup>(defaultBinding, startupConfig, _ => { });
        }

        public static IWebHost Create<TStartup>(string defaultBinding, AppStartupConfig startupConfig, Action<IWebHostBuilder> configAction) where TStartup : class
        {
            var httpBinding = startupConfig["http"] ?? defaultBinding;

            var builder = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls(httpBinding)
                    .ConfigureServices(s => s.AddSingleton(startupConfig))
                    .UseStartup<TStartup>();
            configAction(builder);
            var host = builder.Build();
            return host;
        }

        public static IWebHost Create<TStartup>(string defaultBinding, string[] args, Action<IWebHostBuilder> configAction) where TStartup : class
        {
            var startupConfig = new AppStartupConfig(args);

            return Create<TStartup>(defaultBinding, startupConfig, configAction);
        }
    }
}
