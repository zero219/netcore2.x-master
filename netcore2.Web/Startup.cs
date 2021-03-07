using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using netcore2.Web.Data;
using netcore2.Web.Models.IdentityModels;
using netcore2.Web.Models.IdentityModels.Auth;
using netcore2.Web.Models.IocModels;

namespace netcore2.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        //此方法由运行时调用。使用此方法将服务添加到容器。
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                //此lambda确定给定请求是否需要用户对非必需cookie的同意。
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region (依赖注入)注册服务
            //每次http请求，就创建实例   
            //services.AddScoped<IWelcomeService, WelcomeService>();

            //单例模式，请求只创建唯一一个实例
            services.AddSingleton<IWelcomeService, WelcomeService>();

            //每次请求产生新的实例，轻量级的
            //services.AddTransient<IWelcomeService, WelcomeService>();
            #endregion

            #region Identity
            //连接字符串
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //注册Identity服务
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            //添加授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("仅限管理员", policy => policy.RequireRole("Administrators"));
                options.AddPolicy("仅限联系人", policy => policy.RequireClaim("Contact"));
                options.AddPolicy("仅限About", policy => policy.RequireAssertion(context =>
                {
                    if (context.User.HasClaim(x => x.Type == "About"))
                    {
                        return true;
                    }
                    return false;
                }));
                //自定义的权限
                options.AddPolicy("自定义的权限", policy => policy.AddRequirements(
                    new EmailRequirement("admin@123.com"),
                    new ActionRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, EmailHandler>();
            services.AddSingleton<IAuthorizationHandler, CanContactHandler>();
            services.AddSingleton<IAuthorizationHandler, CanAdminHandler>();
            #endregion

            //全局添加CSRF
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });

            //注册mvc服务(AddMvcCore()只添加最核心的MVC服务,AddMvc()方法添加了所有必须的mvc服务，并且在内部调用AddMvc()方法)
            //service.AddMvcCore();
            services.AddMvc(options =>
            {
                //全局注册CSRF
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region 缓存
            //缓存
            services.AddMemoryCache();
            #endregion

        }

        //此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())//开发环境 
            {
                app.UseDeveloperExceptionPage();//开发环境中错误页面
                app.UseStatusCodePages();//状态码
                app.UseDatabaseErrorPage();//数据库迁移错误页面
                //app.UseWelcomePage();//欢迎页面
            }
            else
            {
                //非开发环境
                app.UseStatusCodePagesWithReExecute("/Home/{0}");//404
                app.UseExceptionHandler("/Home/Error");//自定义
            }
            app.UseStaticFiles();//访问wwwroot文件夹
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"))
            }); ;
            app.UseCookiePolicy();
            //使用授权
            app.UseAuthentication();
            //远程验证
            app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
