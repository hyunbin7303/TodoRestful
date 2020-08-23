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
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                })
                .AddCookie(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
                {
                    options.ReturnUrlParameter = $"returnUrl";
                    options.LoginPath = $"/Account/Login";
                    options.LogoutPath = $"/Account/Logout";
                    options.AccessDeniedPath = $"/Account/AccessDenied";
                });
            });
        }
    }
}
