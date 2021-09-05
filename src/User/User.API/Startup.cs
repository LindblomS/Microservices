namespace Services.User.API
{
    using Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services.User.API.Infrastructure;
    using Services.User.Infrastructure;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConnectionProvider>(sp => new ConnectionProvider("server=(localdb)\\mssqllocaldb;database=identity;integrated security=true;"));

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.ApiName = "user";
                    options.Authority = "https://localhost:5003";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireClaim("scope", "user.write"));
            });

            services.ConfigureApplicationCookie(c =>
            {
                c.Cookie.Name = "IdentityServer.Cookie";
                c.LoginPath = "/Account/Login";
                c.LogoutPath = "/Account/Logout";
            });

            
            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
