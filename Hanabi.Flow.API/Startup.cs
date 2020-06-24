using System;
using System.IO;
using Hanabi.Flow.API.Extensions;
using Hanabi.Flow.Common.Helpers;
using Hanabi.Flow.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Hanabi.Flow.API
{
    public class Startup
    {
        // ���ж��峣��,��������༭
        private const string _apiVersion = "V1.0";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
            
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Configuration));
            services.AddScoped<MyContext>();
            // ConfigureServices��������Ӵ���
            services.AddSwaggerGen(setup =>
            {
                // ����Swagger�ĵ������ƣ�������Ϣ��
                setup.SwaggerDoc(_apiVersion, new OpenApiInfo
                {
                    Version = _apiVersion,
                    Title = "���API˵���ĵ�",
                    Description = "������������API����",
                    Contact = new OpenApiContact { Name = "HANABI", Email = "Narancia86@outlook.com", Url = new Uri("https://colasaikou.com/") },
                    License = new OpenApiLicense { Name = "HANABI", Url = new Uri("https://colasaikou.com/") }
                });

                // ����API���������
                setup.OrderActionsBy(description => description.RelativePath);

                // ���ýӿ�ע����Ϣ
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Hanabi.Flow.API.xml");
                setup.IncludeXmlComments(xmlPath, true);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext myContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", _apiVersion);

                // ��launchSettings.json��launchUrl����Ϊ��,��ʾ��������ʱ���ʸ�����,�����������Swaggerҳ��·��ǰ׺����Ϊ��,������API����������Swagger����
                c.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseDataGenerator(myContext);
        }
    }
}
