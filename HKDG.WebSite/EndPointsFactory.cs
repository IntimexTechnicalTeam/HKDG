namespace HKDG.WebSite
{
    public static class EndPointsFactory
    {      
        /// <summary>
        /// 所有的路由地址绑定在这里实现
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder BindEndPoints(this IEndpointRouteBuilder builder)
        {
            builder.AddEndPoints("Default", "/{controller=Default}/{action=Index}/{IspType?}/{para2?}/{para3?}");     //让MVC Controller支持无参或一个参数以上
            builder.AddEndPoints("Category", "/{controller=Product}/{action=Category}/{IspType?}");                       //让MVC Controller支持无参或一个参数以上
            builder.AddEndPoints("MyMessage", "/{controller=Account}/{action=MyMessage}/{IspType?}");             //让MVC Controller支持无参或一个参数以上
            builder.AddEndPoints("ProductSearch", "/{controller=Product}/{action=Search}/{IspType?}/{key}");
            builder.AddEndPoints("ProductDetail", "/{controller=Product}/{action=Detail}/{IspType?}/{Id}");
            builder.AddEndPoints("MerchantList", "/{controller=Merchant}/{action=List}/{IspType?}/{key?}");
            builder.AddEndPoints("MerchantDetail", "/{controller=Merchant}/{action=Detail}/{IspType?}/{Id}");

            return builder;
        }

        /// <summary>
        /// 增加自定义EndPoints 节点，比如，Mvc--Controller---View
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        static void AddEndPoints(this IEndpointRouteBuilder builder, string name, string pattern)
        {
            builder.MapControllerRoute(name, pattern);
        }

    }
}
