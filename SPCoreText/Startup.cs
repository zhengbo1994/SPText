using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EF_CodeDB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SPCoreText.Extensions;
using SPCoreText.Services;
using SPCoreText.Unlity;
using SPTextLK.Interface;
using SPTextLK.Service;

namespace SPCoreText
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
            // 服务容器 IoC(控制反转,Inversion of Control)容器
            // 注册类型、请求实例

            // 默认已经为我们注册了一些服务,服务，服务容器

            // 添加对控制器和API相关功能的支持，但是不支持视图和页面
            services.AddControllers();

            //// 添加对控制器\API\视图相关功能的支持。（全局注册）（调试有错误）
            //services.AddControllersWithViews(
            //    options =>
            //{
            //    //options.Filters.Add<TextExecptionFilterAttrbute>();//全局注册
            //}).AddRazorRuntimeCompilation();//修改cshtml后能自动编译
            //services.AddAuthentication(
            //    CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = new PathString("/Fourth/Login");
            //        options.AccessDeniedPath = new PathString("/Home/Privacy");

            //    });//用cookie的方式验证，顺便初始化登录地址


            // 添加对Razor Pages和最小控制器的支持
            services.AddRazorPages();

            // 这里是ASP.NET CORE 2.X 
            services.AddMvc();
            services.AddCors();
            // 比较规范的服务封装
            services.AddMessage(builder => builder.UseSms());

            //缓存
            services.AddResponseCaching();

            // 内置的服务

            // 第三方的，EF Core，日志框架、Swagger、

            // 注册自定义服务
            // 服务生存期   类型生命周期

            // 注册自定义服务的时候，必须要选择一个生存周期

            // 有几种生存周期
            // 瞬时，每次从服务容器里进行请求实例时，都会创建一个新的实例。
            // 作用域，线程单例，在同一个线程（请求）里，只实例化一次
            // 单例，全局单例，每一次都是使用相同的实例，

            //生存周期
            //services.AddScoped<TextActionFilterAttribute>();  //作用域注册
            //services.AddSingleton();//单例
            //services.AddTransient();//瞬时

            services.AddSingleton<IMessageService, EmailService>();
            services.AddSingleton<IMessageService, SmsService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddScoped(typeof(TextExecptionFilterAttrbute));  //注册全局异常处理特性
            services.AddScoped<DbContext, DataBaseContext>();//EF数据库映射

            services.AddSession();
            //全局注册
            //services.AddMvc(p =>
            //{
            //    p.Filters.Add(typeof(TextExecptionFilterAttrbute));
            //});

            #region  EF（DbContext）配置


            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbContext"));
            });


            //services.AddEntityFrameworkSqlServer()
            //      .AddDbContext<DataBaseContext>(options =>
            //      {
            //          options.UseSqlServer(Configuration.GetConnectionString("DbContext") //读取配置文件中的链接字符串
            //              );  //配置分页 使用旧方式
            //      });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // 配置中间件
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Middleware 2 Begin \r\n");
            //    await next();
            //    await context.Response.WriteAsync("Middleware 2 End \r\n");
            //});

            // run方法，是没有next的，终端中间件
            // 专门用来短路请求管道，是放在最后面的，兜底的。
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello Run \r\n");
            //});


            // 环境名称Development
            if (env.IsDevelopment())
            {
                // 开发人员异常页面中间件
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //脚本启动，这里需要改
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();
            app.UseSession();

            // 通用的添加中间件的方法
            //app.UseMiddleware<TestMiddleware>();
            //app.UseTest();



            // 终结点中间件，这里是配置，配置中间件和路由的之间关系，映射
            // 终结点你可以简单理解为 MVC, /控制器/action
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
