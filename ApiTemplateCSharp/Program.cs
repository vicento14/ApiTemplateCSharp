using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ApiTemplateCSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
                    // G7x3FaKN5+QgsYe@
                    var key = Encoding.ASCII.GetBytes("Bd00NXIrhM6ttxnH6JjOXBE2jWM+OFTRQktHx/9p1uk=\r\n"); // Use a strong secret key
                    services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = true;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
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
                        //options.AddPolicy("AllowSpecificOrigin",
                        //    builder => builder.WithOrigins("https://example.com", "https://anotherdomain.com")
                        //                      .AllowAnyHeader()
                        //                      .AllowAnyMethod());

                        //options.AddPolicy("AllowSpecificOrigin",
                        //    builder => builder.WithOrigins("https://example.com")
                        //              .WithHeaders("content-type", "authorization")
                        //              .WithMethods("GET", "POST", "PUT", "DELETE");
                    });
                    services.AddControllers()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });
                });
    }
}
