var builder = WebApplication.CreateBuilder(args);

//��һ��ע��
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//�ڶ���ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));

builder.AddNLog().AddJsonFile("Config/appsettings.json") .ConfigureServices();
builder.ConfigureApp((x) => x.Build()).ConfigureSetting().Run();