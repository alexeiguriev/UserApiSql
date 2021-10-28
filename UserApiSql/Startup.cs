// Unused usings removed
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using UserApiSql.Data;
using UserApiSql.Interfaces;
using Microsoft.AspNetCore.Authentication;
using UserApiSql.Handlers;
using UserApiSql.Helpers;
using System;

namespace UserApiSql
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Envirement is in Production mode");
                Console.WriteLine("--> Use user-api-sqls-clusterip-srv DB");
                try
                {
                    services.AddDbContext<UserContext>(opt =>
                            opt.UseSqlServer(Configuration.GetConnectionString("UserContext")));
                    Console.WriteLine("--> Initialized user-api-sqls-clusterip-srv DB");
                }
                catch
                {
                    Console.WriteLine("--> user-api-sqls-clusterip-srv DB Initialization error");
                }
            }
            else
            {
                bool sqlInitialized = false;
                try
                {
                    switch(Configuration.GetSection("DatabaseType").Value)
                    {
                        case "SQL":
                        {
                            Console.WriteLine("--> Envirement is in development mode");
                            Console.WriteLine("--> Use local DB");
                            services.AddDbContext<UserContext>(opt =>
                            opt.UseSqlServer(Configuration.GetConnectionString("UserContext")));
                            sqlInitialized = true;
                            break;
                        }
                        default:
                        {
                            /* Do nothing */
                            break;
                        }

                    }
                }
                catch
                {
                            Console.WriteLine("--> SQL could not initialize ");
                }
                if (false == sqlInitialized)
                {
                            Console.WriteLine("--> Using InMem Db");
                            services.AddDbContext<UserContext>(opt =>
                            opt.UseInMemoryDatabase("InMem"));
                }

            }
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication",null);
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<JwtService>();
            services.AddControllers();
            Console.WriteLine("--> Services configuration: Done");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            bool sqlEn = (env.IsProduction() || (Configuration.GetSection("DatabaseType").Value == "SQL"));
            PrepDb.PrepPopulation(app, sqlEn);
        }
    }
}
