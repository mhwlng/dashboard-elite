using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using dashboard_elite.Hubs;
using dashboard_elite.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;
using MudBlazor.Services;
using Serilog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace dashboard_elite
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMudServices();

            services.AddSingleton<SvgCacheService>();
            services.AddSingleton<ButtonCacheService>();
            services.AddSingleton<ProfileCacheService>();

            services.AddSingleton<Data>();
            services.AddSingleton<Galnet>();
            services.AddSingleton<Poi>();
            services.AddSingleton<HWInfo>();
            services.AddSingleton<Ships>();
            services.AddSingleton<Module>();
            services.AddSingleton<History>();
            services.AddSingleton<Material>();
            services.AddSingleton<Cargo>();
            services.AddSingleton<Missions>();
            services.AddSingleton<Engineer>();
            services.AddSingleton<Route>();
            services.AddSingleton<SystemInfo>();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddHostedService<Worker>();

            //services.Configure<Program.Dimensions>(Configuration.GetSection("Dimensions"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(dashboard_elite.Program.ExePath, @"wwwroot")),
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<MyHub>("/myhub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
