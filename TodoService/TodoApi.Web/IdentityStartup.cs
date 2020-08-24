using Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.Extensions.Options;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Hosting;

[assembly: HostingStartup(typeof(TodoApi.Web.IdentityStartup))]
namespace TodoApi.Web
{
    public class IdentityStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddIdentityCore<ApplicationUser>()
                        .AddUserStore<UserStore>()
                        .AddUserManager<UserManager<ApplicationUser>>()
                        .AddSignInManager();

                services.AddTransient<ApplicationUser>();
                services.AddTransient<UserManager<ApplicationUser>>();
                services.AddTransient<IUserStore<ApplicationUser>, UserStore>();

                // Use this to implement Api protection, without IdentityServer UI
                //services.AddAuthentication("Bearer")
                //    .AddIdentityServerAuthentication("Bearer", options =>
                //    {
                //        options.ApiName = "api1";
                //        options.Authority = "https://localhost:5001";
                //    });

                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                    .AddCookie(IdentityServerConstants.DefaultCookieAuthenticationScheme)
                    .AddOpenIdConnect("oidc", options =>
                    {
                        options.Authority = "https://localhost:5001";
                        options.ClientId = "oidcClient";
                        options.ClientSecret = "SuperSecretPassword";
                        options.RequireHttpsMetadata = false; // set to true in release

                        options.ResponseType = "code";
                        options.UsePkce = true;
                        options.ResponseMode = "query";

                        options.CallbackPath = "/api/todo/success"; // default redirect URI

                        // options.Scope.Add("oidc"); // default scope
                        // options.Scope.Add("profile"); // default scope
                        options.Scope.Add("api1.read");
                        options.SaveTokens = true;
                        options.Events = new OpenIdConnectEvents
                        {
                            OnRedirectToIdentityProvider = redirectContext =>
                            {
                                if (!context.HostingEnvironment.IsEnvironment("Debug"))
                                {
                                    //Force scheme of redirect URI (THE IMPORTANT PART)
                                    redirectContext.ProtocolMessage.RedirectUri = redirectContext.ProtocolMessage.RedirectUri.Replace("http://", "https://", StringComparison.OrdinalIgnoreCase);
                                }
                                return Task.FromResult(0);
                            }
                        };
                    });
            });

            

        }
    }
}
