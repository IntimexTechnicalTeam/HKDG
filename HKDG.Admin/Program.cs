var builder = WebApplication.CreateBuilder(args);

Startup.AddJsonFile(builder, "Config/appsettings.json");
Startup.AddNLog(builder);

//��һ��ע��
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//�ڶ���ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));

//ConfigureServices(builder.Services);
Startup.ConfigureServices(builder);
var app = builder.Build();
Startup.ConfigurePipeLine(app, builder);
Startup.RegisterSetting();
app.Run();
