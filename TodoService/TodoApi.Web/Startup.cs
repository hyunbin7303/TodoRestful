using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoApi.Datasource;
using TodoApi.Model.DailyTask;
using Serilog;
using IdentityServer4;
using Serilog.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Web.Services;
using AutoMapper;

namespace TodoApi.Web
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
            var section = Configuration.GetSection("MongoDbSettings");
            services.Configure<MongoDbSettings>(section);
            services.AddSingleton<IMongoSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            
            services.AddScoped<ITodoService, TodoService>();

            services.AddControllers();

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("Bearer", config =>
                {
                    // provide token validation parameters
                    var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                    var key = new SymmetricSecurityKey(secretBytes);

                    //token sent with parameter
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.ContainsKey("access_token"))
                            {
                                context.Token = context.Request.Query["access_token"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                    
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Constants.Issuer,
                        ValidAudience = Constants.Audiance,
                        ValidateIssuer = true,
                        ValidateAudience = true,

                        IssuerSigningKey = key
                    };

                    config.SaveToken = true;
                });


            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(Infrastructure.Mappings.TodoMapping).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => 
                {
                    await context.Response.WriteAsync("Hello World");
                });
                // Using metadata to configure the audit policy.
                endpoints.MapGet("/sensitive", async context =>
                {
                    await context.Response.WriteAsync("sensitive data");
                })
                .WithMetadata(new AuditPolicyAttribute(needsAudit: true));
                endpoints.MapControllers();
            });
        }

        private interface IHttpRoute
        {
        }
    }
    public class AuditPolicyAttribute : Attribute
    {
        public AuditPolicyAttribute(bool needsAudit)
        {
            NeedsAudit = needsAudit;
        }

        public bool NeedsAudit { get; }
    }
}
