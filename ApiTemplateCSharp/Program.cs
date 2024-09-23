using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ApiTemplateCSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplateCSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hContext, services) =>
                {
                    var config = hContext.Configuration;
                    const string DB_CONTEXT_CONNSTRING = "MySqlConnection1";
                    services.AddDbContext<UserAccountsDbContext>(options =>
                    {
                        options.UseMySQL(config.GetConnectionString(DB_CONTEXT_CONNSTRING));
                    });
                    services.AddDbContext<TT1DbContext>(options =>
                    {
                        options.UseMySQL(config.GetConnectionString(DB_CONTEXT_CONNSTRING));
                    });
                    services.AddDbContext<TT2DbContext>(options =>
                    {
                        options.UseMySQL(config.GetConnectionString(DB_CONTEXT_CONNSTRING));
                    });
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowAllOrigins",
                            builder =>
                            {
                                builder.AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyHeader();
                            });
                    });
                    services.AddControllers()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });
                });
    }
}
