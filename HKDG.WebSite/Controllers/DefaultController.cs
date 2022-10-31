namespace HKDG.WebSite.Controllers
{
    public class DefaultController : BaseMvcController
    {
        IIspProviderBLL ispProviderBLL;

        public DefaultController(IComponentContext service) : base(service)
        {
            ispProviderBLL = Services.Resolve<IIspProviderBLL>();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="Id">IspType</param>
       /// <returns></returns>
       
        public async Task<IActionResult> Index(string Id)
        {
            await InitViewPage(Id);
            return GetActionResult("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}