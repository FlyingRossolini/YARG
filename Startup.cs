using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using YARG.Data;
using YARG.Data.Services;
using YARG.Models;
using YARG.Services;

namespace YARG
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
                //.AddCookie(options =>
                //{
                //    options.LoginPath = "/account/google-login";
                //    options.Cookie.SameSite = SameSiteMode.Lax;
                //})
                //.AddGoogle(options =>
                //{
                //    options.ClientId = _config.GetValue<string>("Authentication:Google:ClientId");
                //    options.ClientSecret = _config.GetValue<string>("Authentication:Google:ClientSecret");
                //    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                //});

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.AddMvc()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization();


            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("fr"),
                        new CultureInfo("es"),
                        new CultureInfo("uk")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            

            services.AddControllersWithViews();

            services.AddScoped<RemoteMeasurementService>();
            services.AddScoped<RemoteHostDBService>();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.ScheduleJob<MixingFanJob>(trigger => trigger
                    .WithIdentity("1 minute Trigger")
                    .WithCronSchedule("0 0/1 * 1/1 * ? *")
                    .WithDescription("At second :00, every minute starting at minute :00, every hour, every day starting on the 1st, every month")
                    );
            });
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.ScheduleJob<MeasurementJob>(trigger => trigger
                    .WithIdentity("5 minute Trigger")
                    .WithCronSchedule("0 0/5 * 1/1 * ? *")
                    .WithDescription("At second :00, every 5 minutes starting at minute :00, every hour, every day starting on the 1st, every month")
                    );
            });


            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.Host_Address = Configuration["ExternalProviders:MailKit:SMTP:Address"];
                options.Host_Port = Convert.ToInt32(Configuration["ExternalProviders:MailKit:SMTP:Port"]);
                options.Host_Username = Configuration["ExternalProviders:MailKit:SMTP:Account"]; ;
                options.Host_Password = Configuration["ExternalProviders:MailKit:SMTP:Password"];
                options.Sender_EMail = Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
                options.Sender_Name = Configuration["ExternalProviders:MailKit:SMTP:SenderName"]; ;
            });
            if (_config.GetValue<string>("QuartzStrings:ExecuteTaskServiceCallSchedulingStatus").Equals("ON"))
            {
                services.AddSingleton<MqttClientProvider>();
                services.AddSingleton(provider => provider.GetRequiredService<MqttClientProvider>().CreateMqttClient());

                services.AddHostedService<MqttSubscriberService>();
                services.AddSingleton<MqttPublisherService>();

            }

            services.AddTransient<SeedData.SeedData>();

            services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = false);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedData.SeedData seedData)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDeveloperExceptionPage();
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", $"default-src 'self'; img-src 'self' data:; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
                context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                context.Response.Headers.Add("Permissions-Policy", "geolocation=(self), microphone=(), camera=()");

                await next();
            });
            //app.UseCookiePolicy();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseRouting();
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });



            seedData.SeedAdminUser();
        }
    }
}