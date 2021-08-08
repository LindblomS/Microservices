namespace Identity.API
{
    using Autofac;
    using Identity.API.Configurations;
    using Identity.API.Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services.Identity.Infrastructure;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConnectionProvider>(sp => new ConnectionProvider("server=(localdb)\\mssqllocaldb;database=identity;integrated security=true;"));
            services.AddDbContext<IdentityContext>(c => c.UseSqlServer("server=(localdb)\\mssqllocaldb;database=identity;integrated security=true;"));

            services.AddIdentity<IdentityUser, IdentityRole>(c =>
            {
                c.Password.RequiredLength = 4;
                c.Password.RequireDigit = false;
                c.Password.RequireNonAlphanumeric = false;
                c.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(c =>
            {
                c.Cookie.Name = "IdentityServer.Cookie";
                c.LoginPath = "/Account/Login";
                c.LogoutPath = "/Account/Logout";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryApiScopes(Resources.GetApiScopes())
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
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
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
