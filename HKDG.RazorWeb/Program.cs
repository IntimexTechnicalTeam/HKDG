//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

//app.Run();


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
builder.Logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
NLog.LogManager.LoadConfiguration("Config/nlog.config");

//第一种注入
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//第二种注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));

//ConfigureServices(builder.Services);
Startup.ConfigureServices(builder);
var app = builder.Build();
Startup.ConfigurePipeLine(app, builder);
app.Run();
