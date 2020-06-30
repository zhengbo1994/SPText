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

            services.AddScoped<DbContext, DataBaseContext>();//�������׻����ǰ��ҷ�װ��һ��
            services.AddScoped<IUserService, UserService>();

            //���ע��û�гɹ�--ע����û����ģ����캯��Ҳֻ��֧�ֲ����ͺã�����ע��ĵط�����дDbContext
            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JDDbConnection"));
            });


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
            //this.Configuration.ConsulRegist();
        }
    }
}
