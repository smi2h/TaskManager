using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using TaskManager.HostHelper;

namespace TaskManager.Api.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var startupConfig = new AppStartupConfig(args);
            var host = WebHostFactory.Create<Startup>("http://*:5060/", startupConfig);

            host.Run();
        }
    }
}
