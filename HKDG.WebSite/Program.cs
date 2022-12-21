var builder = WebApplication.CreateBuilder(args);

//��һ��ע��
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//�ڶ���ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));
builder.AddNLog().AddJsonFile("Config/appsettings.json", "Config/Resource_E.json", "Config/Resource_C.json", "Config/Resource_S.json")
                    .ConfigureServices();

 builder.ConfigureApp((x)=>x.Build()).ConfigureSetting().Run();


