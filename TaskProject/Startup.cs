using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskProject.Models;
using TaskProject.Services;
using TaskProject;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace TaskProject
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
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.Password = new PasswordOptions()
            {
                RequireDigit = true,
                RequiredLength = 8,
                RequireNonAlphanumeric = false,
                RequireLowercase = false,
                RequireUppercase = false
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Google authentification
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "148895869514-jnqmdu4vp5fe4obbbudci13kp6a702fi.apps.googleusercontent.com"  /*Configuration["Authentication:Google:ClientId"]*/;
                options.ClientSecret = "XtHgT-DHK04uIc_cc-XOv3oS" /*Configuration["Authentication:Google:ClientSecret"]*/;
            });


            //// VK authentification
            //services.AddAuthentication().AddVK(options =>
            //{
            //    options.ClientId = "";
            //    options.ClientSecret = "";

            //    // Request for permissions https://vk.com/dev/permissions?f=1.%20Access%20Permissions%20for%20User%20Token
            //    options.Scope.Add("email");

            //    // Add fields https://vk.com/dev/objects/user
            //    options.Fields.Add("uid");
            //    options.Fields.Add("first_name");
            //    options.Fields.Add("last_name");

            //    // In this case email will return in OAuthTokenResponse, 
            //    // but all scope values will be merged with user response
            //    // so we can claim it as field
            //    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            //    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            //    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            //    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");

            //});

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //Обработка ошибок в продакшене
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                app.UseExceptionHandler("/Home/Error");
            }

            // Поддержка статических файлов
            app.UseStaticFiles();

            // Добавляем поддержку каталога node_modules
            // Заккоментировать при публикации на хостинг

            //app.UseFileServer(new FileServerOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(env.ContentRootPath, "node_modules")
            //    ),
            //    RequestPath = "/node_modules",
            //    EnableDirectoryBrowsing = false
            //});

            // Поддержка аутентификации
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
