using System;
using Autofac.Extensions.DependencyInjection;
using carbon.api.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace carbon.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Run();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        private static IWebHost CreateWebHostBuilder(string[] args)
        {
            
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(s => s.AddAutofac())
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();
        }
            
    }
}