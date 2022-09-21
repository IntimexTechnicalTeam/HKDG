



namespace HandleCalculateQtyService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RegisterHelper.RunConfig((services, config) =>
            {
                //依赖注入
                Web.Framework.AutoMapperConfiguration.InitAutoMapper();
                Web.RegisterConfig.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);
                WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);
                HKDG.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);
                Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);
                return services;
            }, args);

        }
    }
}
