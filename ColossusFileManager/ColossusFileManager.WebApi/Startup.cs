using ColossusFileManager.Shared.Interfaces;
using ColossusFileManager.Shared.Models;
using ColossusFileManager.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColossusFileManager.WebApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Get Sqlite Config From appsettings.json
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // Build a compatible string
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = connectionString };            
            var sqliteConnection = new SqliteConnection(connectionStringBuilder.ToString());

            // Setup EF Context
            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>(options => {                
                options.EnableSensitiveDataLogging();
                options.UseSqlite(sqliteConnection);
            }, ServiceLifetime.Transient);

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Add service(s)
            services.AddTransient<IFileManagerService, ColossusFileManagerService>();

            // Setup Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Colossus File Manager",
                    Version = "v1",
                    Description = "A really cool WebAPI for Adding Files and Folders to the Colossus File System",
                    Contact = new OpenApiContact
                    {
                        Name = "Derek Hampton",
                        Email = "derek.hampton@hotmail.com",
                        Url = new Uri("https://projectfinanceexchange.com"),
                    },
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Swagger for easier integrations similar to old-style .wsdl scaffolders.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Colossus File Manager V1");

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Build the database if it doesn't already exist
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
