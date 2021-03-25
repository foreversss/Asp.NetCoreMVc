using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StarupDemo.IService;
using StarupDemo.Service;

namespace StarupDemo
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

            #region ע�����ͬ�������ڵķ���
            //����ģʽ
            services.AddSingleton<IOrderService, OrderService>();

            //˲ʱ
            services.AddTransient<IOrderService, OrderService>();

            //������
            services.AddScoped<IOrderService, OrderService>();

            #endregion


            #region ����ע��

            //ֻҪ���������д˷����� ��ô�Ͳ���ע��
            services.TryAddSingleton<IOrderService, OrderService>();
            
            //һ������������ͬһ�ӿڲ�ͬʵ��
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderService>());


            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
