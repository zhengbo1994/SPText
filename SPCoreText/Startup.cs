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

            //services.AddControllersWithViews().AddRazorRuntimeCompilation();//��̬����

            //// ��ӶԿ�����\API\��ͼ��ع��ܵ�֧�֡���ȫ��ע�ᣩ�������д���
            //services.AddControllersWithViews(
            //    options =>
            //{
            //    //options.Filters.Add<TextExecptionFilterAttrbute>();//ȫ��ע��
            //}).AddRazorRuntimeCompilation();//�޸�cshtml�����Զ�����
            //services.AddAuthentication(
            //    CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = new PathString("/Home/Login");
            //        options.AccessDeniedPath = new PathString("/Home/Privacy");
            //    });//��cookie�ķ�ʽ��֤��˳���ʼ����¼��ַ


            // ��Ӷ�Razor Pages����С��������֧��
            services.AddRazorPages().AddRazorRuntimeCompilation();

            // ������ASP.NET CORE 2.X 


            services.AddMvc();

            #region ֧�ֿ���  ���е�Api��֧�ֿ���
            services.AddCors(option => option.AddPolicy("AllowCors", _build => _build.AllowAnyOrigin().AllowAnyMethod()));
            //services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            #endregion

            #region JWT��Ȩ��Ȩ���贴��һ����������⣩
            ////1.Nuget����������Microsoft.AspNetCore.Authentication.JwtBearer 
            ////services.AddAuthentication();//����  
            //var validAudience = this.Configuration["audience"];
            //var validIssuer = this.Configuration["issuer"];
            //var securityKey = this.Configuration["SecurityKey"];
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //Ĭ����Ȩ�������ƣ�                                      
            //         .AddJwtBearer(options =>
            //         {
            //             options.TokenValidationParameters = new TokenValidationParameters
            //             {
            //                 ValidateIssuer = true,//�Ƿ���֤Issuer
            //                 ValidateAudience = true,//�Ƿ���֤Audience
            //                 ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
            //                 ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
            //                 ValidAudience = validAudience,//Audience
            //                 ValidIssuer = validIssuer,//Issuer���������ǰ��ǩ��jwt������һ��  ��ʾ˭ǩ����Token
            //                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))//�õ�SecurityKey
            //                 //AudienceValidator = (m, n, z) =>
            //                 //{ 
            //                 //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
            //                 //},//�Զ���У����򣬿����µ�¼��֮ǰ����Ч 
            //             };
            //         });





            #endregion

            // �ȽϹ淶�ķ����װ
            services.AddMessage(builder => builder.UseSms());

            //����
            services.AddResponseCaching();

            //��������
            //services.AddScoped<TextActionFilterAttribute>();  //������ע�ᣨ�������̵߳�������ͬһ���̣߳������ֻʵ����һ�Σ�
            //services.AddSingleton();//������������ȫ�ֵ�����ÿһ�ζ���ʹ����ͬ��ʵ������
            //services.AddTransient();//˲ʱ��˲ʱ��ÿ�δӷ����������������ʵ��ʱ�����ᴴ��һ���µ�ʵ������

            services.AddSingleton<IMessageService, EmailService>();
            services.AddSingleton<IMessageService, SmsService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddScoped(typeof(TextExecptionFilterAttrbute));  //ע��ȫ���쳣��������
            services.AddTransient<IUserService, UserService>();

            services.AddSession();

            #region  EF��DbContext������

            services.AddScoped<DbContext, DataBaseContext>();//EF���ݿ�ӳ��

            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbContext"));
            });



            //services.AddEntityFrameworkSqlServer()
            //      .AddDbContext<DataBaseContext>(options =>
            //      {
            //          options.UseSqlServer(Configuration.GetConnectionString("DbContext") //��ȡ�����ļ��е������ַ���
            //              );  //���÷�ҳ ʹ�þɷ�ʽ
            //      });
            #endregion

            #region Filter��ʽ
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

            #region �������֤--�Զ���Handler
            //services.AddAuthenticationCore();
            //services.AddAuthentication().AddCookie();
            //services.AddAuthenticationCore(options => options.AddScheme<CustomHandler>("CustomScheme", "DemoScheme"));
            #endregion

            #region ����Cookie��Ȩ
            //services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            //services.AddMemoryCache();
            //////services.AddDistributedRedisCache(options =>
            //////{
            //////    options.Configuration = "127.0.0.1:6379";
            //////    options.InstanceName = "RedisDistributedSession";
            //////});
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//������
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = "Cookie/Login";
            //})
            //.AddCookie(options =>
            //{
            //    //��Ϣ���ڷ����--��keyд��cookie--����session
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
            //    };//��չ�¼�
            //});

            ////new AuthenticationBuilder().AddCookie()
            #endregion

            #region ����Cookies��Ȩ---��ɫ��Ȩ
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    //������,signin signout Authenticate���ǻ���Scheme
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

            #region ���ڲ�����Ȩ
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//������
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Authorization/Index";
            //    options.AccessDeniedPath = "/Authorization/Index";
            //});

            ////����һ�����õ�policy
            //var qqEmailPolicy = new AuthorizationPolicyBuilder().AddRequirements(new QQEmailRequirement()).Build();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim��Role��Admin
            //        .RequireUserName("Eleven")//Claim��Name��Eleven
            //        .RequireClaim(ClaimTypes.Email)//������ĳ��Cliam
            //        //.Combine(qqEmailPolicy)
            //        );//����

            //    options.AddPolicy("UserPolicy",
            //        policyBuilder => policyBuilder.RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == ClaimTypes.Role)
            //        && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin")
            //   //.Combine(qqEmailPolicy)
            //   );//�Զ���
            //    //policy����  û��Requirements


            //    //options.AddPolicy("QQEmail", policyBuilder => policyBuilder.Requirements.Add(new QQEmailRequirement()));
            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder.Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
            #endregion


            #region  ������Ŀ��ֲ�������ã�
            //services.AddDistributedMemoryCache();
            ////������1.���AddLocalization
            //services.AddLocalization(options => options.ResourcesPath = "Resources");


            //services.AddMemoryCache();
            //services.AddOptions();

            ////ӳ�������ļ�
            //services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));


            //ʹ��HttpContext��ȡ����չ ������ȡ��̬HttpContext
            services.AddHttpContextAccessorExt();

            ////ʹ��AutoFac��������ע��
            //new AutofacServiceProvider(AutofacExt.InitAutofac(services));
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // �����м��
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Middleware 2 Begin \r\n");
            //    await next();
            //    await context.Response.WriteAsync("Middleware 2 End \r\n");
            //});

            // run��������û��next�ģ��ն��м��
            // ר��������·����ܵ����Ƿ��������ģ����׵ġ�
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello Run \r\n");
            //});


            loggerFactory.AddLog4Net();
            app.UseSession();

            // ��������Development
            if (env.IsDevelopment())
            {
                // ������Ա�쳣ҳ���м��
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

            //�ű�������������Ҫ��
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
            });

            #region ֧�ֿ���
            app.UseCors("AllowCors");
            #endregion

            #region ͨ���м����֧�ּ�Ȩ��Ȩ
            //app.UseAuthentication(); //���߿�� Ҫʹ��Ȩ����֤
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();

            // ͨ�õ�����м���ķ���
            //app.UseMiddleware<TestMiddleware>();
            //app.UseTest();

            #region  ������Ŀ��ֲ�������ã�
            ////������3.Ӧ��UseRequestLocalization
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
            ////ʹ�þ�̬��HttpContext
            ////2. Ϊ�Լ������ľ�̬HttpContext��ӵ�ǰapp��HttpContext������
            app.UseStaticHttpContext();
            #endregion



            // �ս���м�������������ã������м����·�ɵ�֮���ϵ��ӳ��
            // �ս������Լ����Ϊ MVC, /������/action
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
