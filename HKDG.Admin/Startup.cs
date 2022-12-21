using HKDG.Repository;
using Intimex.Runtime;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace HKDG.Admin
{
    public static class Startup
    {
        /// <summary>
        /// ע��Json�ļ�
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        public static WebApplicationBuilder AddJsonFile(this WebApplicationBuilder builder,params string[] path)
        {
            foreach (var item in path)
            {
                builder.Configuration.AddJsonFile(item, optional: true, reloadOnChange: true).AddEnvironmentVariables();
            }
            return builder;
        }

        /// <summary>
        /// ע��Nlog��־���
        /// </summary>
        /// <param name="builder"></param>
        public static WebApplicationBuilder AddNLog(this WebApplicationBuilder builder)
        {
            builder.Logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("Config/nlog.config");
            return builder;
        }

        // ��IServiceCollection������ע��ȫ������
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
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
            builder.Services.AddScoped(typeof(AdminApiAuthorizeAttribute));         //ע��Filter          

            builder.Services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //ȫ�ּ�Ȩ              
                options.EnableEndpointRouting = false;
            });

            ConfigureApiBehaviorOptions(builder.Services);

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            builder.Services.AddSingleton(builder.Configuration);

            WebCache.ServiceCollectionExtensions.AddCacheServices(builder.Services, builder.Configuration);                        //ע��redis���
            HKDG.Repository.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                      //ע��EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                                    //ע��RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(builder.Services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(builder.Services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(builder.Services, typeof(Program));
            Web.Mvc.ServiceCollectionExtensions.AddFileProviderServices(builder.Services, builder.Configuration);

            builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));    //�˾���Ϊ�˽����ͼ�ӻ�ȡ�Ӻ�̨���������ݵ�ʱ�����������޷�����
            builder.Services.AddUEditorService("Config/config.json");
            //AddScopedIServiceProvider(services);
            return builder;
        }

        // ���� HTTP request pipeline 
        public static WebApplication ConfigureApp(this WebApplicationBuilder builder ,Func<WebApplicationBuilder, IApplicationBuilder> func)
        {
            var app = func.Invoke(builder);
            Globals.Services = app.ApplicationServices;
            Globals.Configuration = builder.Configuration;

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
            //app.UseEndpoints(endpoints =>
            //{             
            //    endpoints.MapControllerRoute(
            //       name: "default",
            //       //pattern: "{controller=Home}/{action=Index}/{id?}");
            //       pattern: "{controller=Account}/{action=Login}/{id?}/{para2?}/{para3?}");             //��MVC Controller֧���޲λ�һ����������

            //    endpoints.MapAreaControllerRoute(
            //             name: "areas",
            //             areaName: "AdminApi",
            //             pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{para2?}");      //��Api Controller֧���޲λ�һ����������

            //});

            app.UseEndpoints(endpoints => { endpoints = endpoints.BindEndPoints(); });
			return (WebApplication)app;
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

        /// <summary>
        /// ע��ȫ������
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication ConfigureSetting(this WebApplication app)
        {            
            var baseRepository = app.Services.Resolve<IBaseRepository>();
            var master = baseRepository.GetModel<CodeMaster>(x => x.Module == CodeMasterModule.Setting.ToString() 
                         && x.Function == CodeMasterFunction.Time.ToString() && x.Key == "UserTokenExpire")?.Value ?? "";

            if (!master.IsEmpty())
            {
                if (int.TryParse(master, out var time))
                    Setting.MemberAccessTokenExpire = time;
            }

            Setting.BuyDongWebUrl = app.Configuration["BuyDongWebUrl"];
            return app;
        }
    }
}
