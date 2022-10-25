namespace HKDG.WebSite
{
    /// <summary>
    /// 自定义的视图路径
    /// </summary>
    public class RegisterViewFactory
    {
        public static string[] Views = { 
            "/Views/Desktop/{1}/{0}.cshtml", "/Views/Mobile/{1}/{0}.cshtml" ,
            //"/Views/Desktop/Product/Category.cshtml", "/Views/Mobile/Product/MobileCategory.cshtml"
        };
    }
}
