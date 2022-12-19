var builder = WebApplication.CreateBuilder(args);

Startup.AddNLog(builder);
Startup.AddJsonFile(builder, "Config/appsettings.json", "Config/Resource_E.json", "Config/Resource_C.json", "Config/Resource_S.json");

//第一种注入
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//第二种注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));

Startup.ConfigureServices(builder);
var app = builder.Build();
Startup.ConfigurePipeLine(app, builder);
Startup.RegisterSetting();
app.Run();
