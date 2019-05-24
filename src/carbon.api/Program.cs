using System;
using Autofac.Extensions.DependencyInjection;
using carbon.api.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(s => s.AddAutofac())
                .UseStartup<Startup>()
                .Build();
    }
}