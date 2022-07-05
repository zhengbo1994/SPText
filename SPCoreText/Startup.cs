using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using EF_CodeDB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SPCoreApiText.Utiltiy;
using SPCoreText.Common;
using SPCoreText.Extensions;
using SPCoreText.Interface;
using SPCoreText.Model;
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

            //services.AddControllersWithViews().AddRazorRuntimeCompilation();//动态编译

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
            services.AddRazorPages().AddRazorRuntimeCompilation();

            // 这里是ASP.NET CORE 2.X 


            services.AddMvc();

            #region 支持跨域  所有的Api都支持跨域
            services.AddCors(option => option.AddPolicy("AllowCors", _build => _build.AllowAnyOrigin().AllowAnyMethod()));
            //services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
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

            #region Filter方式
            //services.AddAuthentication()
            //.AddCookie();
            #endregion

            #region Authorize
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie();
            #endregion

            #region 最基础认证--自定义Handler
            //services.AddAuthenticationCore();
            //services.AddAuthentication().AddCookie();
            //services.AddAuthenticationCore(options => options.AddScheme<CustomHandler>("CustomScheme", "DemoScheme"));
            #endregion

            #region 基于Cookie鉴权
            //services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            //services.AddMemoryCache();
            //////services.AddDistributedRedisCache(options =>
            //////{
            //////    options.Configuration = "127.0.0.1:6379";
            //////    options.InstanceName = "RedisDistributedSession";
            //////});
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//不能少
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = "Cookie/Login";
            //})
            //.AddCookie(options =>
            //{
            //    //信息存在服务端--把key写入cookie--类似session
            //    options.SessionStore = services.BuildServiceProvider().GetService<ITicketStore>();
            //    options.Events = new CookieAuthenticationEvents()
            //    {
            //        OnSignedIn = new Func<CookieSignedInContext, Task>(
            //            async context =>
            //            {
            //                Console.WriteLine($"{context.Request.Path} is OnSignedIn");
            //                await Task.CompletedTask;
            //            }),
            //        OnSigningIn = async context =>
            //         {
            //             Console.WriteLine($"{context.Request.Path} is OnSigningIn");
            //             await Task.CompletedTask;
            //         },
            //        OnSigningOut = async context =>
            //        {
            //            Console.WriteLine($"{context.Request.Path} is OnSigningOut");
            //            await Task.CompletedTask;
            //        }
            //    };//扩展事件
            //});

            ////new AuthenticationBuilder().AddCookie()
            #endregion

            #region 基于Cookies授权---角色授权
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    //不能少,signin signout Authenticate都是基于Scheme
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Authorization/Index";
            //    options.AccessDeniedPath = "/Authorization/Index";
            //});
            ////.AddCookie("CustomScheme", options =>
            ////{
            ////    options.LoginPath = "/Authorization/Index";
            ////    options.AccessDeniedPath = "/Authorization/Index";
            ////});
            #endregion

            #region 基于策略授权
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//不能少
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Authorization/Index";
            //    options.AccessDeniedPath = "/Authorization/Index";
            //});

            ////定义一个共用的policy
            //var qqEmailPolicy = new AuthorizationPolicyBuilder().AddRequirements(new QQEmailRequirement()).Build();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim的Role是Admin
            //        .RequireUserName("Eleven")//Claim的Name是Eleven
            //        .RequireClaim(ClaimTypes.Email)//必须有某个Cliam
            //        //.Combine(qqEmailPolicy)
            //        );//内置

            //    options.AddPolicy("UserPolicy",
            //        policyBuilder => policyBuilder.RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == ClaimTypes.Role)
            //        && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin")
            //   //.Combine(qqEmailPolicy)
            //   );//自定义
            //    //policy层面  没有Requirements


            //    //options.AddPolicy("QQEmail", policyBuilder => policyBuilder.Requirements.Add(new QQEmailRequirement()));
            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder.Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
            #endregion


            #region  其它项目移植（不可用）
            //services.AddDistributedMemoryCache();
            ////多语言1.添加AddLocalization
            //services.AddLocalization(options => options.ResourcesPath = "Resources");


            //services.AddMemoryCache();
            //services.AddOptions();

            ////映射配置文件
            //services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));


            //使用HttpContext获取器扩展 用来获取静态HttpContext
            services.AddHttpContextAccessorExt();

            ////使用AutoFac进行依赖注入
            //new AutofacServiceProvider(AutofacExt.InitAutofac(services));
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

            #region  其它项目移植（不可用）
            ////多语言3.应用UseRequestLocalization
            //IList<CultureInfo> supportedCultures = new List<CultureInfo>
            //{
            //    new CultureInfo("en-US"),
            //    new CultureInfo("zh-CN"),
            //    new CultureInfo("zh-HK")
            //};
            //app.UseRequestLocalization(new RequestLocalizationOptions()
            //{
            //    DefaultRequestCulture = new RequestCulture("en-US"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //});

            //app.UseCookiePolicy();
            //app.UseSession();
            ////使用静态的HttpContext
            ////2. 为自己构建的静态HttpContext添加当前app的HttpContext访问器
            app.UseStaticHttpContext();
            #endregion



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
