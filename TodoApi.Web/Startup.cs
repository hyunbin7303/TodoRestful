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
using TodoApi.Model.Chores;
using TodoApi.Model.DailyTask;
using TodoApi.Model.Jobs;
using Serilog;

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
            services.AddControllers();
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
