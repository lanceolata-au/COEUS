using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using AutoMapper;
using carbon.api.Features;
using carbon.api.Services;
using carbon.persistence.modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace carbon.api
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            
            /*
             * Identity Server 4
             * http://docs.identityserver.io
             *
             * Used for client credentials and authorisation access
             *
             * OpenID/oAuth2 based for ASP.net
             */

            //START =-=-= DO NOT MODIFY UNLESS DISCUSSED USER AUTH IS HERE =-=-= START

            
            var store = new X509Store(StoreLocation.CurrentUser);

            var signingCert = new X509Certificate2();

            var storeFind = store.Certificates.Find(
                X509FindType.FindBySubjectDistinguishedName,
                "CN={" + Configuration.GetSection("X509Details").GetSection("CertificateName").Value + "}",
                false);
            
            if (storeFind.Count > 0)
            {
                signingCert = new X509Certificate2(storeFind.Export(X509ContentType.Pfx));
            } 
            else
            {
                signingCert = X509Utilities.BuildSelfSignedServerCertificate(
                    Configuration.GetSection("X509Details").GetSection("CertificateName").Value,
                    Configuration.GetSection("X509Details").GetSection("DecryptionPassword").Value);
                store.Certificates.Add(signingCert);
            }
            
            Console.WriteLine("ConfigureServices Start");

            var appUri = Configuration.GetSection("Hosts").GetSection("APPFqdn").Value;
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(appUri)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            
            //Startup Autofac
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<ApplicationIdentityDbContext>(options 
                => options.UseMySql(Configuration.GetConnectionString("ApplicationDatabase"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();
            
            services
                .AddIdentityServer(options =>
                {
                    options.PublicOrigin = Configuration.GetSection("Hosts").GetSection("APIFqdn").Value;
                })
                .AddOperationalStore(options =>
                {
                    options
                        .ConfigureDbContext = optionsBuilder => optionsBuilder
                        .UseMySql(Configuration.GetConnectionString("ApplicationDatabase"),
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 604800; //stay logged in for 1 week
                })
                .AddConfigurationStore(options => options
                    .ConfigureDbContext = optionsBuilder => optionsBuilder
                    .UseMySql(Configuration.GetConnectionString("ApplicationDatabase"),sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddSigningCredential(signingCert);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetSection("Hosts").GetSection("APIFqdn").Value;
                options.Audience = "carbon.api";
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
            });

            services.AddMvc();

            //TODO this is soon to be deprecated. Find a new solution.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            Console.WriteLine("ConfigureServices Completed");

            //  END =-=-= DO NOT MODIFY UNLESS DISCUSSED USER AUTH IS HERE =-=-= END
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Console.WriteLine("Configure Start");
            
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                IdentityModelEventSource.ShowPII = true; 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //START =-=-= DO NOT MODIFY UNLESS DISCUSSED USER AUTH IS HERE =-=-= START
            
            IdentitySetup.InitializeDatabase(app, Configuration);
            
            app.UseCors();

            app.UseIdentityServer();

            app.UseAuthentication();

            //END =-=-= DO NOT MODIFY UNLESS DISCUSSED USER AUTH IS HERE =-=-= END
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            Console.WriteLine("Configure End");
            
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Console.WriteLine("ConfigureContainer Start");

            builder.RegisterModule(new Persistence());
            builder.RegisterAssemblyModules(AppScanner.GetCarbonAssemblies());
            
            Console.WriteLine("ConfigureContainer End");
        }
    }
}