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
            services.AddRazorPages();

            // ������ASP.NET CORE 2.X 


            services.AddMvc();

            #region ֧�ֿ���  ���е�Api��֧�ֿ���
            services.AddCors(option => option.AddPolicy("AllowCors", _build => _build.AllowAnyOrigin().AllowAnyMethod()));
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
