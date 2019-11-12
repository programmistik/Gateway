using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyIdentityService.Areas.Identity.Data;
using MyIdentityService.Models;

[assembly: HostingStartup(typeof(MyIdentityService.Areas.Identity.IdentityHostingStartup))]
namespace MyIdentityService.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MyIdentityServiceContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MyIdentityServiceContextConnection")));

                services.AddDefaultIdentity<MyIdentityServiceUser>()
                    .AddEntityFrameworkStores<MyIdentityServiceContext>();
            });
        }
    }
}