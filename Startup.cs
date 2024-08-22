using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using JobApplicationApi.Data;
using Microsoft.Extensions.Logging;
// using JobApplicationApi.Repositories;


namespace JobApplicationApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            // services.AddScoped<ApplicationStageRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobApplicationApi", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder => builder
                        .WithOrigins("http://localhost:4200") 
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAngularApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobApplicationApi v1");
            });

            // Global error handling
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Something went wrong: {ex}");
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
