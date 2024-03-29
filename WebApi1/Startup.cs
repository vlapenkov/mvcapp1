using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mvcapp;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using shared;
using Shared;
using WebApi1.Quartz;

namespace WebApi1
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
            services.AddSingleton<IProblemDetailsLogger, ProblemDetailsLogger>();
            services.AddTransient<IExceptionHandler, DefaultExceptionHandler>();
            services.AddDbContext<ProductsDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("ProductsDatabase")));


            services.AddHttpContextAccessor();
            services.AddTransient<ExampleJob>();


            services.ConfigureQuartz();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // .AddAuthorization(options => options.AddPolicy("Founder", policy => policy.RequireClaim("Employee", "Mosalla")))


            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Authority = "http://localhost:5500";
                   options.RequireHttpsMetadata = false;
                   options.Audience = "Api1";
               });
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("DefaultPolicy", policy =>
                    {
                        policy.AuthenticationSchemes.Add("Bearer");
                        policy.RequireAuthenticatedUser();
                        // policy.Requirements.Add(new MinimumAgeRequirement());
                    });
                }
                );



            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<CorrelationIdEnrichLogMiddleware>();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())

            //{

            //    var context = serviceScope.ServiceProvider.GetService<ProductsDbContext>();

            //    context.Database.Migrate();

            //}
        }


    }
}
