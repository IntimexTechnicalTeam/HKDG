var builder = WebApplication.CreateBuilder(args);

//第一种注入
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//第二种注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));

builder.AddNLog().AddJsonFile("Config/appsettings.json") .ConfigureServices();
builder.ConfigureApp((x) => x.Build()).ConfigureSetting().Run();