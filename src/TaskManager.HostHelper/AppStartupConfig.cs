using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace TaskManager.HostHelper
{
    public class AppStartupConfig
    {
        public IConfigurationRoot StartupConfiguration { get; }

        public AppStartupConfig(string[] commandLineArgs)
        {
            var defaultLocation = Path.Combine(Directory.GetCurrentDirectory(), DefaultConfigsPath);
            StartupConfiguration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string> { { ConfigsPathKeyName, defaultLocation } })
                    .Build();
        }

        public string ConfigsPath => this[ConfigsPathKeyName];

        public string this[string param] => StartupConfiguration.GetSection(param).Value;

        public static string DefaultConfigsPath => "../../config";

        public static string ConfigsPathKeyName { get; set; } = "configs-path";
    }
}
