using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EF_CodeDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using SPCoreApiText.Utiltiy;
using SPCoreTextLK.Interface;
using SPCoreTextLK.Service;

namespace SPCoreApiText
{
    //Install-Package Swashbuckle.AspNetCore -Version 5.0.0-rc4
    //https://localhost:44398/swagger/index.html
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

            services.AddScoped<DbContext, DataBaseContext>();//下面这套还不是把我封装了一下
            services.AddScoped<IUserService, UserService>();

            //这个注入没有成功--注入是没问题的，构造函数也只是支持参数就好，错在注入的地方不能写DbContext
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JDDbConnection"));
            });


            services.AddTransient<ITestServiceA, TestServiceA>();//瞬时
            services.AddSingleton<ITestServiceB, TestServiceB>();//单例
                                                                 //services.AddScoped<ITestServiceC, TestServiceC>();//作用域单例--一次请求一个实例
                                                                 //作用域其实依赖于ServiceProvider（这个自身是根据请求的），跟多线程没关系
                                                                 //services.AddTransient<ITestServiceD, TestServiceD>();
                                                                 //services.AddTransient<ITestServiceE, TestServiceE>();

            #region 注册swagger
            services.AddSwaggerGen(c =>
                  {
                      c.SwaggerDoc("v1", new OpenApiInfo
                      {
                          Title = "云小励平台接口",
                          Version = "1",
                          //Description = "这是描述",
                          //TermsOfService = new Uri("https://www.baidu.com"),
                          //License = new OpenApiLicense()
                          //{
                          //    Name = "许可证",
                          //    Url = new Uri("https://www.baidu.com"),
                          //},
                          //Contact = new OpenApiContact()
                          //{
                          //    Name = "郑博",
                          //    Email = "179720610@qq.com",
                          //    Url = new Uri("https://www.baidu.com"),
                          //}
                      });
                      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                      c.IncludeXmlComments(xmlPath);
                  });
            #endregion

            #region  网关
            //services.AddOcelot();
            //services.AddOcelot().AddConsul().AddPolly();
            #endregion

            #region jwt校验
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = this.Configuration["audience"],//Audience
                    ValidIssuer = this.Configuration["issuer"],//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["SecurityKey"])),//拿到SecurityKey
                    //AudienceValidator = (m, n, z) =>
                    //{
                    //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                    //},//自定义校验规则，可以新登录后将之前的无效
                };
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region  Swagger规范和Swagger UI
            //Swagger规范和Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "云小励平台(V 1)");
            });
            #endregion

            app.UseHttpsRedirection();

            #region jwt
            app.UseAuthentication();//注意添加这一句，启用验证
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region  网关
            //app.UseOcelot();//网关
            #endregion
            //实例启动时执行，且只执行一次
            //this.Configuration.ConsulRegist();
        }
    }
}
