using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EliteToyhauler.v3.Data;
using EliteToyhauler.v3.Dmp64;
using EliteToyhauler.v3.Application;
using EliteToyhauler.v3.Dmp64.Service;
using EliteToyhauler.v3.Dmp64.Client;
using EliteToyhauler.v3.Application.Audio;
using EliteToyhauler.v3.Sensor;

namespace EliteToyhauler.v3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.Configure<Dmp64Settings>(Configuration.GetSection("Dmp64").Bind);

            services.AddSingleton<IDataStore, DataStore>();
            services.AddSingleton<IDmp64TcpClient, Dmp64TcpClient>(); 
            services.AddSingleton<IDmp64Service, Dmp64Service>();
            services.AddTransient<Temperature>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
