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
        // ��IServiceCollection������ע��ȫ������
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    // ����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    // ��ʹ���շ�
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    // ����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })               
                .AddRazorRuntimeCompilation();              //�����ڱ������ģʽ�±༭view
            //builder.Services.AddScoped(typeof(UserAuthorizeAttribute));         //ע��Filter

            //׷��API ��������
            SwaggerExtension.AddSwagger(builder.Services, "HKDG.WebSite.xml", "HKDG.Domain.xml");

            builder.Services.Configure<MvcViewOptions>(configureOptions =>
            {
                var viewEngines = configureOptions.ViewEngines; //��ͼ����
                //���ڴ˴���չ��ͼ����
            });
            builder.Services.Configure<RazorViewEngineOptions>(configureOptions =>
            {
                configureOptions.ViewLocationExpanders.Add(new TemplateViewLocationExpander(() => {
                    return RegisterViewFactory.Views;                   
                } )); //��ͼĬ��·����չ
            });

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(UserAuthorizeAttribute));            //ȫ�ּ�Ȩ
                options.EnableEndpointRouting = false;
            });

            ConfigureApiBehaviorOptions(builder.Services);

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            builder.Services.AddSingleton(builder.Configuration);

            WebCache.ServiceCollectionExtensions.AddCacheServices(builder.Services, builder.Configuration);                        //ע��redis���
            Repository.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                      //ע��EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                                    //ע��RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(builder.Services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(builder.Services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(builder.Services, typeof(Program));
            Web.Mvc.ServiceCollectionExtensions.AddFileProviderServices(builder.Services, builder.Configuration);

            builder.Services.AddUEditorService("Config/config.json");
        }

        // ���� HTTP request pipeline 
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

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //ȫ���쳣����
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();      //����HTTPS�ض���

            #region �����ļ�����·��
            var staticFiles = new StaticFileOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(Path.Combine(builder.Configuration["UploadPath"], "ClientResources"))
                 //new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot"))                    
                 ),
                RequestPath = "/ClientResources"      //�������ã������ϴ�������·���·��ʲ���
            };
            app.UseStaticFiles(staticFiles);
            app.UseStaticFiles();
            #endregion

            app.UseRouting();
            app.UseAuthorization();         //���������UseRouting �� UseEndpoints ֮��
            app.UseEndpoints(endpoints => { endpoints = endpoints.BindEndPoints(); });
        }
        /// <summary>
        /// ģ����֤
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

                    //��ȡ��֤ʧ�ܵ�ģ���ֶ� 
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
