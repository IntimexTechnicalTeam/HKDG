﻿namespace HKDG.BLL
{
    public class TestService :BaseBLL, ITestService
    {
        //IProductService productService;


        //public IUserService user { get; set; }

        public TestService(IServiceProvider services) : base(services)
        {
            //productService = this.Services.Resolve<IProductService>();
        }

        public  void Hello(string msg)
        {
            //productService = this.Services.Resolve<IProductService>();
            //productService.GetProductList();

            //var userService = this.Services.Resolve<IUserService>();

            var aa = this.Services.Resolve<IProductBLL>();


            // userService.GetUser();
            Console.WriteLine(msg);
            //return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}
