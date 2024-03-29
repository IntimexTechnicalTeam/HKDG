﻿namespace HKDG.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class UserController: WebController
    {
        IUserBLL userBLL;

        public UserController(IComponentContext services) : base(services)
        {
            userBLL = Services.Resolve<IUserBLL>();
        }

        [HttpGet]
        public async Task<SystemResult> CreateMerchantAccount(string merchantIds)
        {
            SystemResult result = userBLL.CreateAccountForMerchant(merchantIds);          
            return result;
        }

        [HttpPost]
        public SystemResult Search([FromForm]UserCondition condition)
        {
            var result = new SystemResult();
            var pageData = userBLL.Search(condition);
            result.ReturnValue = pageData;
            result.Succeeded = true;

            return result;
        }

        [HttpGet]
        public SystemResult Get(Guid id)
        {
            var user = userBLL.GetById(id);
            SystemResult systemResult = new SystemResult();
            systemResult.ReturnValue = user;
            systemResult.Succeeded = true;
            return systemResult;
        }

        [HttpGet]
        public SystemResult Remove(string id)
        {
            SystemResult result = userBLL.Remove(Guid.Parse(id));
            return result;
        }

        [HttpGet]
        public SystemResult ActRemove(string id)
        {
            var user = userBLL.GetById(new Guid(id));
            SystemResult result = userBLL.PhysicalDelete(user);
            return result;
        }

        [HttpGet]
        public SystemResult ResetPassword(string id)
        {
            SystemResult result =userBLL.ResetPassword(new Guid(id));
            return result;
        }

        [HttpPost]

        public SystemResult Save([FromForm]UserDto model)
        {
            SystemResult result = new SystemResult();
            if (model.Id == Guid.Empty)
            {
                result = userBLL.Save(model);
            }
            else
            {
                result = userBLL.Update(model);
            }
            return result;
        }

        [HttpPost]
        public SystemResult UpdatePassword(PasswordView passwordView)
        {
            SystemResult result= userBLL.UpdatePassword(passwordView);
            return result;
        }
    }
}
