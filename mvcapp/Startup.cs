using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mvc.services;
using Refit;
using Shared;
using System;
using WebApi1.Contracts.Interfaces;

namespace mvcapp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        //private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        //{
        //    if (options.SameSite == SameSiteMode.None)
        //    {
        //        //var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        //        //if (MyUserAgentDetectionLib.DisallowsSameSiteNone(userAgent))
        //        {
        //            //      options.Domain = "";
        //            options.SameSite = SameSiteMode.Unspecified;
        //        }
        //    }
        //}
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // нужен дл€ нормальной работы OpenIdConnect в Chrome 8 без https.
            //“акже нужен код в IS4 
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            //    options.OnAppendCookie = cookieContext =>
            //        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            //    options.OnDeleteCookie = cookieContext =>
            //        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);

            //});


            services.AddHttpContextAccessor();
            services.AddSingleton<IProblemDetailsLogger, ProblemDetailsLogger>();
            services.AddTransient<IExceptionHandler, DefaultExceptionHandler>();
            services.AddTransient<mvc.services.AccessTokenHandler>();
            services.AddTransient<ErrorMessageHandler>();


            #region identity
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";

            })
               .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    // чтобы User.Identity.Name работало
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:5500";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("Api1.Scope");
                    options.Scope.Add("Api2.Scope");

                    options.Scope.Add("phone");
                    options.Scope.Add("custom.profile");

                    options.Scope.Add("offline_access");
                });

            #endregion 
            services.AddControllersWithViews()
                //.AddNewtonsoftJson(x => {
                //x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                //x.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                //})
                ;

            var baseAddress = new Uri(Configuration["Services:WebApi1"]);
            services.AddRefitClient<IProductService>().ConfigureHttpClient(c => c.BaseAddress = baseAddress)
                .AddHttpMessageHandler<mvc.services.AccessTokenHandler>()
                .AddHttpMessageHandler<ErrorMessageHandler>();

            services.AddRefitClient<IWeatherService>().ConfigureHttpClient(c => c.BaseAddress = baseAddress)
               .AddHttpMessageHandler<mvc.services.AccessTokenHandler>()
               .AddHttpMessageHandler<ErrorMessageHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // нужен дл€ нормальной работы OpenIdConnect в Chrome 8 без https.
            //“акже нужен код в IS4 

            app.UseCookiePolicy();
            ////app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            TryCatchExtensions.Handler = app.ApplicationServices.GetService<IExceptionHandler>();
            // первый самый гибкий способ, можно логировать и обрабатывать как захочетс€
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));



            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
