using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace SPApiText
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

            //ע��entity
            //services.addDbContext

            //ע��swagger
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Swagger�淶��Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "��С��ƽ̨(V 1)");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
