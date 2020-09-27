using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_CodeDB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SPCoreText.Extensions;
using SPCoreText.Services;
using SPCoreText.Unlity;
using SPCoreTextLK.Interface;
using SPCoreTextLK.Service;
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
            //        options.LoginPath = new PathString("/Home/Login");
            //        options.AccessDeniedPath = new PathString("/Home/Privacy");
            //    });//用cookie的方式验证，顺便初始化登录地址


            // 添加对Razor Pages和最小控制器的支持
            services.AddRazorPages();

            // 这里是ASP.NET CORE 2.X 


            services.AddMvc();

            #region 支持跨域  所有的Api都支持跨域
            services.AddCors(option => option.AddPolicy("AllowCors", _build => _build.AllowAnyOrigin().AllowAnyMethod()));
            #endregion

            #region JWT鉴权授权（需创建一个独立的类库）
            ////1.Nuget引入程序包：Microsoft.AspNetCore.Authentication.JwtBearer 
            ////services.AddAuthentication();//禁用  
            //var validAudience = this.Configuration["audience"];
            //var validIssuer = this.Configuration["issuer"];
            //var securityKey = this.Configuration["SecurityKey"];
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //默认授权机制名称；                                      
            //         .AddJwtBearer(options =>
            //         {
            //             options.TokenValidationParameters = new TokenValidationParameters
            //             {
            //                 ValidateIssuer = true,//是否验证Issuer
            //                 ValidateAudience = true,//是否验证Audience
            //                 ValidateLifetime = true,//是否验证失效时间
            //                 ValidateIssuerSigningKey = true,//是否验证SecurityKey
            //                 ValidAudience = validAudience,//Audience
            //                 ValidIssuer = validIssuer,//Issuer，这两项和前面签发jwt的设置一致  表示谁签发的Token
            //                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))//拿到SecurityKey
            //                 //AudienceValidator = (m, n, z) =>
            //                 //{ 
            //                 //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
            //                 //},//自定义校验规则，可以新登录后将之前的无效 
            //             };
            //         });





            #endregion

            // 比较规范的服务封装
            services.AddMessage(builder => builder.UseSms());

            //缓存
            services.AddResponseCaching();

            //生存周期
            //services.AddScoped<TextActionFilterAttribute>();  //作用域注册（作用域，线程单例，在同一个线程（请求）里，只实例化一次）
            //services.AddSingleton();//单例（单例，全局单例，每一次都是使用相同的实例，）
            //services.AddTransient();//瞬时（瞬时，每次从服务容器里进行请求实例时，都会创建一个新的实例。）

            services.AddSingleton<IMessageService, EmailService>();
            services.AddSingleton<IMessageService, SmsService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddScoped(typeof(TextExecptionFilterAttrbute));  //注册全局异常处理特性
            services.AddTransient<IUserService, UserService>();

            services.AddSession();

            #region  EF（DbContext）配置

            services.AddScoped<DbContext, DataBaseContext>();//EF数据库映射

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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


            loggerFactory.AddLog4Net();
            app.UseSession();

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

            #region 支持跨域
            app.UseCors("AllowCors");
            #endregion

            #region 通过中间件来支持鉴权授权
            //app.UseAuthentication(); //告诉框架 要使用权限认证
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();


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

                //endpoints.MapAreaControllerRoute(
                //    name: "areas", "areas",
                //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
