var builder = WebApplication.CreateBuilder(args);

Startup.AddJsonFile(builder, "Config/appsettings.json");
Startup.AddNLog(builder);

//第一种注入
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//第二种注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));

//ConfigureServices(builder.Services);
Startup.ConfigureServices(builder);
var app = builder.Build();
Startup.ConfigurePipeLine(app, builder);
Startup.RegisterSetting();
app.Run();
