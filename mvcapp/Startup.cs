using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mvc.services;
using Refit;
using Shared;
using Shared.Interfaces;
using System;

namespace mvcapp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                //var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                //if (MyUserAgentDetectionLib.DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // нужен дл€ нормальной работы OpenIdConnect в Chrome 8 без https.
            //“акже нужен код в IS4 
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });


            services.AddHttpContextAccessor();
            services.AddTransient<IExceptionHandler,DefaultExceptionHandler>();
            services.AddTransient<MyAccessTokenHandler>();
          //  services.AddTransient<AccessTokenDelegatingHandler>();
            //  JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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

           var baseAddress= new Uri(Configuration["Services:WebApi1"]);
            services.AddRefitClient<IProductService>().ConfigureHttpClient(c => c.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MyAccessTokenHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // нужен дл€ нормальной работы OpenIdConnect в Chrome 8 без https.
            //“акже нужен код в IS4 

            app.UseCookiePolicy();
            //app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            TryCatchExtensions.Handler = app.ApplicationServices.GetService<IExceptionHandler>();
                   // первый самый гибкий способ, можно логировать и обрабатывать как захочетс€
                   app.UseMiddleware(typeof(ErrorHandlingMiddleware));



            // second way exception handling
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    //                app.UseHsts();
            //}

            
                // третий путь всегда логирует принудительно сам
            //    app.UseExceptionHandler(errorApp =>
            //{
              
            //    // Normally you'd use MVC or similar to render a nice page.
            //    errorApp.Run(async context =>
            //    {
                    

            //        context.Response.StatusCode = 500;
            //        context.Response.ContentType = "text/html";
            //        await context.Response.WriteAsync("<html><body>\r\n");
            //        await context.Response.WriteAsync("We're sorry, we encountered an un-expected issue with your application.<br>\r\n");

            //        var error = context.Features.Get<IExceptionHandlerFeature>();
            //        if (error != null)
            //        {
            //            // This error would not normally be exposed to the client
            //            await context.Response.WriteAsync("<br>Error: " + HtmlEncoder.Default.Encode(error.Error.Message) + "<br>\r\n");
            //        }
            //        await context.Response.WriteAsync("<br><a href=\"/\">Home</a><br>\r\n");
            //        await context.Response.WriteAsync("</body></html>\r\n");
            //        await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
            //    });
            //});
            //       app.UseHttpsRedirection();


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
