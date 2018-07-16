﻿using Microsoft.AspNetCore.Builder;
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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace TaskProject
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Указываем файл конфигурации в зависимости от названия конфигурации в переменных среды
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            // Идентификация пароля
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

            // Аутентификация Google
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration.GetSection("Google_API").GetSection("ClientId").Value;
                options.ClientSecret = Configuration.GetSection("Google_API").GetSection("ClientSecret").Value;
            });


            // Аутентификация Facebook
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = Configuration.GetSection("Facebook_API").GetSection("ClientId").Value;
                options.AppSecret = Configuration.GetSection("Facebook_API").GetSection("ClientSecret").Value;
            });

            // Почта.
            services.AddTransient<IEmailSender, EmailSender>();

            // MVC
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
                app.UseBrowserLink();

                // Добавление поддержки ошибок для разработки
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Добавляем поддержку каталога node_modules
                // Заккоментировать при публикации на хостинг

                app.UseFileServer(new FileServerOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(env.ContentRootPath, "node_modules")
                    ),
                    RequestPath = "/node_modules",
                    EnableDirectoryBrowsing = false
                });
            }
            else
            {
                //Редирект с HTTP на HTTPS
                var rewriteoptions = new RewriteOptions()
    .AddRedirectToHttpsPermanent();
                app.UseRewriter(rewriteoptions);

                //Обработка ошибок в продакшене
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                app.UseExceptionHandler("/Home/Error");


                // Открыть доступ для регистрации LetsEncrypt
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
                    RequestPath = new PathString("/.well-known"),
                    ServeUnknownFileTypes = true
                });
            }

            // Добавление кеширования статических файлов
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
                }
            });

            // Поддержка аутентификации
            app.UseAuthentication();

            // Поддержка MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
