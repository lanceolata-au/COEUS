using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using carbon.api.Features;
using carbon.api.Models;
using carbon.api.Services;
using carbon.core.features;
using carbon.persistence.modules;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

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

            Config.Version = Configuration.GetSection("Misc").GetSection("Version").Value;
            
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
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

            X509Certificate2 signingCert;

            if (File.Exists(Configuration.GetSection("X509Details").GetSection("PathToFile").Value))
            {
                signingCert = new X509Certificate2(
                    Configuration.GetSection("X509Details").GetSection("PathToFile").Value,
                    Configuration.GetSection("X509Details").GetSection("DecryptionPassword").Value);
            } 
            else
            {
                signingCert = X509Utilities.BuildSelfSignedServerCertificate(
                    Configuration.GetSection("X509Details").GetSection("CertificateName").Value,
                    Configuration.GetSection("X509Details").GetSection("DecryptionPassword").Value);
                
                File.WriteAllBytes(Configuration.GetSection("X509Details").GetSection("PathToFile").Value,
                    signingCert.Export(X509ContentType.Pkcs12,
                    Configuration.GetSection("X509Details").GetSection("DecryptionPassword").Value));
            }
            
            Console.WriteLine("ConfigureServices Start");

            var appUri = Configuration.GetSection("Hosts").GetSection("APPFqdn").Value;
            var apiUri = Configuration.GetSection("Hosts").GetSection("APIFqdn").Value;

            ApplicationInfo.AppUrl = appUri;
            ApplicationInfo.ApiUrl = apiUri;
            
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

            services.AddMvcCore()
                .AddAuthorization();
            
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();
            
            services
                .AddIdentityServer(options =>
                {
                    options.PublicOrigin = apiUri;
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
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });

            //TODO this is soon to be deprecated. Find a new solution.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            //  END =-=-= DO NOT MODIFY UNLESS DISCUSSED USER AUTH IS HERE =-=-= END

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CeOuS API",
                    Description = "Carbon Event Scout",
                    TermsOfService = new Uri(appUri + "/privacy"),
                    Contact = new OpenApiContact
                    {
                        Name = "Owen Holloway",
                        Email = string.Empty,
                        Url = new Uri("https://zeryter.xyz"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri(appUri + "/privacy"),
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            }
                        },
                        new string[] {}

                    }
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{apiUri}/connect/authorize"),
                            TokenUrl = new Uri($"{apiUri}/connect/token"),
                            
                            Scopes = new Dictionary<string, string>()
                            {
                                { "carbon.read", "carbon API read" },
                                { "carbon.write", "carbon API write" }
                            }
                        }
                    }
                });
                
                
            });

            Console.WriteLine("ConfigureServices Completed");

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

            if (env.IsDevelopment())
            {
                app.UseSwagger();
            
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CeOuS V1");
                });
            }

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