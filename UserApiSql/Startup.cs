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
                services.AddDbContext<UserContext>(opt =>
                            opt.UseSqlServer(Configuration.GetConnectionString("UserContext")));
            }
            else
            {
                Console.WriteLine("--> Envirement is in development mode");
                Console.WriteLine("--> Use local DB");
                services.AddDbContext<UserContext>(opt =>
                            opt.UseSqlServer(Configuration.GetConnectionString("UserContext")));
            }
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication",null);
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<JwtService>();
            services.AddControllers();
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
        }
    }
}
