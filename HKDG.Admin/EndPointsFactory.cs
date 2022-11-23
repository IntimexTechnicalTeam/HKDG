namespace HKDG.Admin
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
            builder.AddEndPoints("Default", "/{controller=Account}/{action=Login}/{Id?}/{para2?}/{para3?}");     //让MVC Controller支持无参或一个参数以上
            
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
