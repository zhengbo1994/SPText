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
    //dotnet SPCoreApiText.dll --urls=��http://*:5726�� �C-ip=��127.0.0.1�� --port=5726
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
            services.AddScoped<DbContext, DataBaseContext>();//�������׻����ǰ��ҷ�װ��һ��
            //���ע��û�гɹ�--ע����û����ģ����캯��Ҳֻ��֧�ֲ����ͺã�����ע��ĵط�����дDbContext
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JDDbConnection"));
            });
            #endregion

            services.AddTransient<ITestServiceA, TestServiceA>();//˲ʱ
            services.AddSingleton<ITestServiceB, TestServiceB>();//����
                                                                 //services.AddScoped<ITestServiceC, TestServiceC>();//��������--һ������һ��ʵ��
                                                                 //��������ʵ������ServiceProvider����������Ǹ�������ģ��������߳�û��ϵ
                                                                 //services.AddTransient<ITestServiceD, TestServiceD>();
                                                                 //services.AddTransient<ITestServiceE, TestServiceE>();

            #region ע��swagger
            services.AddSwaggerGen(c =>
                  {
                      c.SwaggerDoc("v1", new OpenApiInfo
                      {
                          Title = "��С��ƽ̨�ӿ�",
                          Version = "1",
                          //Description = "��������",
                          //TermsOfService = new Uri("https://www.baidu.com"),
                          //License = new OpenApiLicense()
                          //{
                          //    Name = "���֤",
                          //    Url = new Uri("https://www.baidu.com"),
                          //},
                          //Contact = new OpenApiContact()
                          //{
                          //    Name = "֣��",
                          //    Email = "179720610@qq.com",
                          //    Url = new Uri("https://www.baidu.com"),
                          //}
                      });
                      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                      c.IncludeXmlComments(xmlPath);
                  });
            #endregion

            #region  ����
            //services.AddOcelot();
            //services.AddOcelot().AddConsul().AddPolly();
            #endregion

            #region jwtУ��
            services.AddScoped<IJWTService, JWTService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//�Ƿ���֤Issuer
                    ValidateAudience = true,//�Ƿ���֤Audience
                    ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    ValidAudience = this.Configuration["audience"],//Audience
                    ValidIssuer = this.Configuration["issuer"],//Issuer���������ǰ��ǩ��jwt������һ��
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["SecurityKey"])),//�õ�SecurityKey
                    //AudienceValidator = (m, n, z) =>
                    //{
                    //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                    //},//�Զ���У����򣬿����µ�¼��֮ǰ����Ч
                };
            });
            #endregion


            #region  JWT
            #region jwtУ��  HS
            //JWTTokenOptions tokenOptions = new JWTTokenOptions();
            //Configuration.Bind("JWTTokenOptions", tokenOptions);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
            //        ValidateIssuer = true,//�Ƿ���֤Issuer
            //        ValidateAudience = true,//�Ƿ���֤Audience
            //        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
            //        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
            //        ValidAudience = tokenOptions.Audience,//
            //        ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),//�õ�SecurityKey
            //        //AudienceValidator = (m, n, z) =>
            //        //{
            //        //    //��ͬ��ȥ��չ����Audience��У�����---��Ȩ
            //        //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
            //        //},
            //        //LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
            //        //{
            //        //    return notBefore <= DateTime.Now
            //        //    && expires >= DateTime.Now;
            //        //    //&& validationParameters
            //        //}//�Զ���У�����
            //    };
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim��Role��Admin
            //        .RequireUserName("Eleven")//Claim��Name��Eleven
            //        .RequireClaim("EMail")//������ĳ��Cliam
            //         .RequireClaim("Account")
            //        //.Combine(qqEmailPolicy)
            //        .AddRequirements(new CustomExtendRequirement())
            //        );//����

            //    //options.AddPolicy("QQEmail", policyBuilder => policyBuilder.Requirements.Add(new QQEmailRequirement()));
            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder
            //    .AddRequirements(new CustomExtendRequirement())
            //    .Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, CustomExtendRequirementHandler>();
            #endregion

            #region jwtУ��  RS
            //#region ��ȡ��Կ
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
            //        ValidateIssuer = true,//�Ƿ���֤Issuer
            //        ValidateAudience = true,//�Ƿ���֤Audience
            //        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
            //        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
            //        #region MyRegion
            //        //ValidAudience = this.Configuration["Audience"],//Audience
            //        //ValidIssuer = this.Configuration["Issuer"],//Issuer���������ǰ��ǩ��jwt������һ��
            //        #endregion
            //        ValidAudience = this.Configuration["JWTTokenOptions:Audience"],//Audience
            //        ValidIssuer = this.Configuration["JWTTokenOptions:Issuer"],//Issuer���������ǰ��ǩ��jwt������һ��
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
            //        },//�Զ���У����򣬿����µ�¼��֮ǰ����Ч
            //    };
            //});
            #endregion
            #endregion

            #region  ��������ע��
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

            #region  Swagger�淶��Swagger UI
            //Swagger�淶��Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "��С��ƽ̨(V 1)");
            });
            #endregion

            app.UseHttpsRedirection();

            #region jwt
            app.UseAuthentication();//ע�������һ�䣬������֤
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region  ����
            //app.UseOcelot();//����
            #endregion
            //ʵ������ʱִ�У���ִֻ��һ��
            //this.Configuration.ConsulRegist();//����ע���뷢��
        }
    }
}
