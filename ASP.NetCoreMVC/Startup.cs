using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreMVC.Utility.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ASP.NetCoreMVC
{
    /// <summary>
    /// Startup是kestrel跟MVC得关联
    /// </summary>
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
            services.AddControllersWithViews();
        }


        //此方法由运行时调用。使用此方法配置HTTP请求管道。
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// HTTP请求管道:就是http请求处理的过程
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //RequestDelegate;

            #region middleware
            app.Use(next =>
            {
                Console.WriteLine("middleware 1 out");

                return new RequestDelegate(async context =>
                {
                    await context.Response.WriteAsync("This is middleware 1 Start");
                    await next.Invoke(context);
                    await context.Response.WriteAsync("This is middleware 1 End");
                });
            });

            app.Use(next =>
            {
                Console.WriteLine("middleware 2 out");

                return new RequestDelegate(async context =>
                {
                    await context.Response.WriteAsync("This is middleware 2 Start");
                    await next.Invoke(context);
                    //await context.Response.WriteAsync("This is middleware 2 End");
                });
            });

            app.Use(next =>
            {
                Console.WriteLine("middleware 3 out");

                return new RequestDelegate(async context =>
                {
                    await context.Response.WriteAsync("This is middleware 3 Start");

                    await context.Response.WriteAsync("This is middleware 3 End");
                });
            });

            //首先第一阶段是初始化，调用Build ---得到RequestDelegate ---middleware1 ----这个就是管道
            //然后等着请求来 ----contex ---就一层层调用



            #endregion

            #region 中间件源码
            //            using System;
            //            using System.Collections.Generic;
            //            using System.Linq;
            //            using System.Threading.Tasks;
            //            using Microsoft.AspNetCore.Http;
            //            using Microsoft.AspNetCore.Http.Endpoints;
            //            using Microsoft.AspNetCore.Http.Features;
            //            using Microsoft.AspNetCore.Http.Internal;
            //            using Microsoft.Extensions.Internal;

            //namespace Microsoft.AspNetCore.Builder.Internal
            //    {
            //        public class ApplicationBuilder : IApplicationBuilder
            //        {
            //            private readonly IList<Func<RequestDelegate, RequestDelegate>> _components = new List<Func<RequestDelegate, RequestDelegate>>();

            //            public ApplicationBuilder(IServiceProvider serviceProvider)
            //            {
            //                Properties = new Dictionary<string, object>(StringComparer.Ordinal);
            //                ApplicationServices = serviceProvider;
            //            }

            //            public ApplicationBuilder(IServiceProvider serviceProvider, object server)
            //                : this(serviceProvider)
            //            {
            //                SetProperty(Constants.BuilderProperties.ServerFeatures, server);
            //            }

            //            private ApplicationBuilder(ApplicationBuilder builder)
            //            {
            //                Properties = new CopyOnWriteDictionary<string, object>(builder.Properties, StringComparer.Ordinal);
            //            }

            //            public IServiceProvider ApplicationServices
            //            {
            //                get
            //                {
            //                    return GetProperty<IServiceProvider>(Constants.BuilderProperties.ApplicationServices);
            //                }
            //                set
            //                {
            //                    SetProperty<IServiceProvider>(Constants.BuilderProperties.ApplicationServices, value);
            //                }
            //            }

            //            public IFeatureCollection ServerFeatures
            //            {
            //                get
            //                {
            //                    return GetProperty<IFeatureCollection>(Constants.BuilderProperties.ServerFeatures);
            //                }
            //            }

            //            public IDictionary<string, object> Properties { get; }

            //            private T GetProperty<T>(string key)
            //            {
            //                object value;
            //                return Properties.TryGetValue(key, out value) ? (T)value : default(T);
            //            }

            //            private void SetProperty<T>(string key, T value)
            //            {
            //                Properties[key] = value;
            //            }

            //            public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
            //            {
            //                _components.Add(middleware);
            //                return this;
            //            }

            //            public IApplicationBuilder New()
            //            {
            //                return new ApplicationBuilder(this);
            //            }

            //            public RequestDelegate Build()
            //            {
            //                RequestDelegate app = context =>
            //                {
            //                    // Implicitly execute matched endpoint at the end of the pipeline instead of returning 404
            //                    var endpointRequestDelegate = context.GetEndpoint()?.RequestDelegate;
            //                    if (endpointRequestDelegate != null)
            //                    {
            //                        return endpointRequestDelegate(context);
            //                    }

            //                    context.Response.StatusCode = 404;
            //                    return Task.CompletedTask;
            //                };

            //                foreach (var component in _components.Reverse())
            //                {
            //                    app = component(app);
            //                }

            //                return app;
            //            }
            //        }
            //    }

            #endregion


            #region 自定义中间件
            // app.UseMiddleware<ReferrerStealingMiddleware>();
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
