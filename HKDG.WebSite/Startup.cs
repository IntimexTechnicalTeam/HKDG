using Autofac.Core;
using HKDG.Repository;
using Intimex.Runtime;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Model;
using NETCoreViewLocationExpander.ViewLocationExtend;
using System.Xml.Linq;
using Web.Swagger;

namespace HKDG.WebSite
{
    public class Startup
    {
        // 在IServiceCollection容器中注册全局设置
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    // 忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    // 不使用驼峰
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    // 设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // 如字段为null值，该字段不会返回到前端
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })               
                .AddRazorRuntimeCompilation();              //可以在编译调试模式下编辑view
            //builder.Services.AddScoped(typeof(UserAuthorizeAttribute));         //注入Filter

            //追加API 参数描述
            SwaggerExtension.AddSwagger(builder.Services, "HKDG.WebSite.xml", "HKDG.Domain.xml");

            builder.Services.Configure<MvcViewOptions>(configureOptions =>
            {
                var viewEngines = configureOptions.ViewEngines; //视图引擎
                //可在此处扩展视图引擎
            });
            builder.Services.Configure<RazorViewEngineOptions>(configureOptions =>
            {
                configureOptions.ViewLocationExpanders.Add(new TemplateViewLocationExpander(() => {
                    return RegisterViewFactory.Views;                   
                } )); //视图默认路径扩展
            });

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(UserAuthorizeAttribute));            //全局鉴权
                options.EnableEndpointRouting = false;
            });

            ConfigureApiBehaviorOptions(builder.Services);

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            builder.Services.AddSingleton(builder.Configuration);

            WebCache.ServiceCollectionExtensions.AddCacheServices(builder.Services, builder.Configuration);                        //注入redis组件
            Repository.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                      //注入EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                                    //注入RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(builder.Services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(builder.Services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(builder.Services, typeof(Program));
            Web.Mvc.ServiceCollectionExtensions.AddFileProviderServices(builder.Services, builder.Configuration);

            builder.Services.AddUEditorService("Config/config.json");
        }

        // 设置 HTTP request pipeline 
        public static void ConfigurePipeLine(IApplicationBuilder app, WebApplicationBuilder builder)
        {
            Globals.Services = app.ApplicationServices;
            Globals.Configuration = builder.Configuration;

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Default/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureSwagger();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //全局异常处理
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();      //开启HTTPS重定向

            #region 设置文件保存路径
            var staticFiles = new StaticFileOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(Path.Combine(builder.Configuration["UploadPath"], "ClientResources"))
                 //new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot"))                    
                 ),
                RequestPath = "/ClientResources"      //必须设置，否则上传完后相对路径下访问不了
            };
            app.UseStaticFiles(staticFiles);
            app.UseStaticFiles();
            #endregion

            app.UseRouting();
            app.UseAuthorization();         //这个必须在UseRouting 和 UseEndpoints 之间
            app.UseEndpoints(endpoints => { endpoints = endpoints.BindEndPoints(); });
        }
        /// <summary>
        /// 模型验证
        /// </summary>
        /// <param name="services"></param>
        static void ConfigureApiBehaviorOptions(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {

                    var parmList = actionContext.ActionDescriptor.Parameters;

                    //(ControllerParameterDescriptor)

                    //获取验证失败的模型字段 
                    var errors = actionContext.ModelState
                     .Where(e => e.Value.Errors.Count > 0)
                     .Select(e => e.Value.Errors.First().ErrorMessage)
                     .ToList();
                    string str = errors.FirstOrDefault();
                    var result = new SystemResult
                    {
                        Succeeded = false,
                        Message = str,
                    };
                    return new OkObjectResult(result);
                };
            });
        }

        public static void RegisterSetting()
        {            
            var baseRepository = Globals.Services.Resolve<IBaseRepository>();
            var master = baseRepository.GetModel<CodeMaster>(x => x.Module == CodeMasterModule.Setting.ToString() 
                         && x.Function == CodeMasterFunction.Time.ToString() && x.Key == "MemTokenExpire")?.Value ?? "";

            if (!master.IsEmpty())
            {
                if (int.TryParse(master, out var time))
                    Setting.MemberAccessTokenExpire = time;
            }

            Setting.BuyDongWebUrl = Globals.Configuration["BuyDongWebUrl"];
        }
    }
}
