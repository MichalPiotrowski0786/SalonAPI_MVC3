using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Salon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salon
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDistributedMemoryCache();

            services.AddControllersWithViews().AddSessionStateTempDataProvider();
            services.AddRazorPages().AddSessionStateTempDataProvider();

            services.AddAuthorization(options => {
                options.AddPolicy("readpolicy",
                    builder => builder.RequireRole("Admin", "User"));
                options.AddPolicy("writepolicy",
                    builder => builder.RequireRole("Admin"));
            });

            // sesje
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Salon.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.IsEssential = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            // routing
            app.UseEndpoints(endpoints =>
            {
                //route dla /cars/details/id
                endpoints.MapControllerRoute(
                    name: "salon",
                    pattern: "samochody/szczegoly/{id?}",
                    defaults: new
                    {
                        controller = "Cars",
                        action = "Details"
                    }
                    );

                //route dla /cars/create
                endpoints.MapControllerRoute(
                    name: "salonCreate",
                    pattern: "samochody/dodaj",
                    defaults: new
                    {
                        controller = "Cars",
                        action = "Create"
                    }
                    );

                //route dla /cars/edit/id
                endpoints.MapControllerRoute(
                    name: "salonEdit",
                    pattern: "samochody/edytuj/{id?}",
                    defaults: new
                    {
                        controller = "Cars",
                        action = "Edit"
                    }
                    );

                //route dla /cars/delete/id
                endpoints.MapControllerRoute(
                    name: "salonDelete",
                    pattern: "samochody/usun/{id?}",
                    defaults: new
                    {
                        controller = "Cars",
                        action = "Delete"
                    }
                    );

                //domyœlne przekierowanie na /cars
                endpoints.MapControllerRoute(
                    name: "salonIndex",
                    pattern: "{controller=Cars}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            // obs³uga b³êdu 404
            app.UseStatusCodePagesWithRedirects("/cars");
        }
    }
}
