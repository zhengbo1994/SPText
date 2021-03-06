using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EF_CodeDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using SPCoreApiText.Utiltiy;
using SPCoreTextLK.Interface;
using SPCoreTextLK.Service;

namespace SPCoreApiText
{
    //dotnet SPCoreApiText.dll --urls=”http://*:5726” –-ip=”127.0.0.1” --port=5726
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


            services.AddScoped<IUserService, UserService>();

            #region  EF
            services.AddScoped<DbContext, DataBaseContext>();//下面这套还不是把我封装了一下
            //这个注入没有成功--注入是没问题的，构造函数也只是支持参数就好，错在注入的地方不能写DbContext
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JDDbConnection"));
            });
            #endregion

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
            services.AddScoped<IJWTService, JWTService>();
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


            #region  JWT
            #region jwt校验  HS
            //JWTTokenOptions tokenOptions = new JWTTokenOptions();
            //Configuration.Bind("JWTTokenOptions", tokenOptions);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //JWT有一些默认的属性，就是给鉴权时就可以筛选了
            //        ValidateIssuer = true,//是否验证Issuer
            //        ValidateAudience = true,//是否验证Audience
            //        ValidateLifetime = true,//是否验证失效时间
            //        ValidateIssuerSigningKey = true,//是否验证SecurityKey
            //        ValidAudience = tokenOptions.Audience,//
            //        ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),//拿到SecurityKey
            //        //AudienceValidator = (m, n, z) =>
            //        //{
            //        //    //等同于去扩展了下Audience的校验规则---鉴权
            //        //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
            //        //},
            //        //LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
            //        //{
            //        //    return notBefore <= DateTime.Now
            //        //    && expires >= DateTime.Now;
            //        //    //&& validationParameters
            //        //}//自定义校验规则
            //    };
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim的Role是Admin
            //        .RequireUserName("Eleven")//Claim的Name是Eleven
            //        .RequireClaim("EMail")//必须有某个Cliam
            //         .RequireClaim("Account")
            //        //.Combine(qqEmailPolicy)
            //        .AddRequirements(new CustomExtendRequirement())
            //        );//内置

            //    //options.AddPolicy("QQEmail", policyBuilder => policyBuilder.Requirements.Add(new QQEmailRequirement()));
            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder
            //    .AddRequirements(new CustomExtendRequirement())
            //    .Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, CustomExtendRequirementHandler>();
            #endregion

            #region jwt校验  RS
            //#region 读取公钥
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "key.public.json");
            //string key = File.ReadAllText(path);//this.Configuration["SecurityKey"];
            //Console.WriteLine($"KeyPath:{path}");

            //var keyParams = JsonConvert.DeserializeObject<RSAParameters>(key);
            //foreach (var item in keyParams.GetType().GetFields())
            //{
            //    Console.WriteLine($"{item.Name}:{item.GetValue(keyParams)}");
            //}
            ////var credentials = new SigningCredentials(new RsaSecurityKey(keyParams), SecurityAlgorithms.RsaSha256Signature);
            //#endregion

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,//是否验证Issuer
            //        ValidateAudience = true,//是否验证Audience
            //        ValidateLifetime = true,//是否验证失效时间
            //        ValidateIssuerSigningKey = true,//是否验证SecurityKey
            //        #region MyRegion
            //        //ValidAudience = this.Configuration["Audience"],//Audience
            //        //ValidIssuer = this.Configuration["Issuer"],//Issuer，这两项和前面签发jwt的设置一致
            //        #endregion
            //        ValidAudience = this.Configuration["JWTTokenOptions:Audience"],//Audience
            //        ValidIssuer = this.Configuration["JWTTokenOptions:Issuer"],//Issuer，这两项和前面签发jwt的设置一致
            //        IssuerSigningKey = new RsaSecurityKey(keyParams),
            //        IssuerSigningKeyValidator = (m, n, z) =>
            //         {
            //             Console.WriteLine("This is IssuerSigningKeyValidator");
            //             return true;
            //         },
            //        //IssuerValidator = (m, n, z) =>
            //        // {
            //        //     Console.WriteLine($"This is IssuerValidator {this.Configuration["JWTTokenOptions:Issuer"]}");
            //        //     return "http://localhost:5726";
            //        // },
            //        AudienceValidator = (m, n, z) =>
            //        {
            //            Console.WriteLine("This is AudienceValidator");
            //            return true;
            //            //return m != null && m.FirstOrDefault().Equals(this.Configuration["Audience"]);
            //        },//自定义校验规则，可以新登录后将之前的无效
            //    };
            //});
            #endregion
            #endregion

            #region  反射依赖注入
            var assembly = Assembly.Load("");
            var types = assembly.GetTypes().Where(p => !p.GetTypeInfo().IsInterface&& !p.GetTypeInfo().IsEnum);
            foreach (var itemType in types)
            {
                foreach (var itemInterface in itemType.GetInterfaces())
                {
                    services.AddScoped(itemInterface, itemType);
                }
            }
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
            //this.Configuration.ConsulRegist();//服务注册与发现
        }
    }
}
