using System;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduleTask.Data;
using ScheduleTask.Interface;
using ScheduleTask.Services;
using ScheduleTask.Utils;
using SmartBreadcrumbs.Extensions;
using WebMarkupMin.AspNetCore3;

namespace ScheduleTask
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
            services.AddScoped<ITaskService,TaskService>();
            services.AddScoped<INecessaryTime,NecessaryTimeService>();

            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(15);
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserService>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<CustomIdentityError>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });
            
            services.AddDNTCaptcha(options =>
            {
                options.UseCookieStorageProvider(SameSiteMode.Strict) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                    // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                    // .UseDistributedSerializationProvider()

                    .AbsoluteExpiration(minutes: 7)
                    .ShowThousandsSeparators(false)
                    .WithNoise(pixelsDensity: 25, linesCount: 3)
                    .WithEncryptionKey("کد امنیتی!")
                    .InputNames(// This is optional. Change it if you don't like the default names.
                        new DNTCaptchaComponent
                        {
                            CaptchaHiddenInputName = "DNTCaptchaText",
                            CaptchaHiddenTokenName = "DNTCaptchaToken",
                            CaptchaInputName = "DNTCaptchaInputText"
                        })
                    .Identifier("dntCaptcha")// This is optional. Change it if you don't like its default name.
                    ;
            });
            
            services.AddBreadcrumbs(GetType().Assembly);
            services.AddWebMarkupMin(options =>
            {
                options.AllowCompressionInDevelopmentEnvironment=true;
                options.AllowMinificationInDevelopmentEnvironment=true;
            }).AddHtmlMinification()
                .AddHttpCompression();
            
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseWebMarkupMin();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}