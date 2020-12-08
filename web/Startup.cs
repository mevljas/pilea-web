using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Models;

namespace web
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
            services.AddControllersWithViews();

            // Register PileaContext as a service
            services.AddDbContext<PileaContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("AzureContext")));


            services.AddIdentity<User, IdentityRole>(
                options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<PileaContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();  //For login, register and others page templates.
            });

        
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
            // This one didn't work
            // c.SwaggerEndpoint("/api/v1/swagger.json", "My API V1");

            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned Api v1");
            });

        }
    }
}
