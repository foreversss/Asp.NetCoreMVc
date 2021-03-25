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

            #region 注册服务不同生命周期的服务
            //单例模式
            services.AddSingleton<IOrderService, OrderService>();

            //瞬时
            services.AddTransient<IOrderService, OrderService>();

            //作用域
            services.AddScoped<IOrderService, OrderService>();

            #endregion


            #region 尝试注册

            //只要容器里面有此服务了 那么就不会注册
            services.TryAddSingleton<IOrderService, OrderService>();
            
            //一个容器可以有同一接口不同实现
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
