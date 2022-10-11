using Autofac;
using HKDG.BLL;
using HKDG.Domain;
using HKDG.Enums;
using HKDG.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Framework;
using Web.Mvc;

namespace HKDG.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.MemberModule)]
    public class MemberController : BaseApiController
    {
        public IMemberBLL memberBLL;

        public MemberController(IComponentContext services) : base(services)
        {
            memberBLL = Services.Resolve<IMemberBLL>();
        }

        /// <summary>
        /// 以table的形式顯示會員信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]

        public PageData<MemberDto> MemberTableData(MbrSearchCond condition)
        {
            PageData<MemberDto> data = new PageData<MemberDto>();
            //List<MemberInfo> list = new List<MemberInfo>();
            try
            {
                if (condition == null)
                {
                    return null;
                }
                //MemSearchCond cond = new MemSearchCond();
                //IMemberBLL membBLL = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();

                //cond = condition.Condition;
                condition.Language = condition.Language == null ? new List<string>() : condition.Language;
                condition.EmailAdd = condition.EmailAdd == null ? "" : condition.EmailAdd.Trim();
                condition.Code = condition.Code == null ? "" : condition.Code.Trim();
                condition.FirstName = condition.FirstName == null ? "" : condition.FirstName.Trim();
                condition.LastName = condition.LastName == null ? "" : condition.LastName.Trim();
                condition.Mobile = condition.Mobile == null ? "" : condition.Mobile.Trim();


                data = memberBLL.Search(condition);

            }
            catch (Exception ex)
            {

            }

            return data;
        }

        /// <summary>
        ///  查询会员列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]
        public SystemResult Search([FromBody] MbrSearchCond condition)
        {
            var result = new SystemResult() { Succeeded = true };

            var testUser = CurrentUser;

            result.ReturnValue = memberBLL.SearchMember(condition);
            return result;
        }

        [HttpGet]

        public List<MbrSpendingRank> GetMbrSpendingRank()
        {
            List<MbrSpendingRank> data = new List<MbrSpendingRank>();


            //IMemberBLL membBLL = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();
            data = memberBLL.GetMbrSpendingRank();

            return data;
        }




        /// <summary>
        /// 根據id獲取會員信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetMember(Guid id)
        {
            MemberDto membDto = new MemberDto();

            //IMemberBLL bll = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();

            membDto = await memberBLL.GetMember(id);

            return Json(membDto);
        }

        /// <summary>
        /// 根據id獲取是否拒收邮件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool GetMemberOpt(Guid id)
        {


            var memopt = memberBLL.IsOptOutPromotion(id);

            return memopt;
        }



        /// <summary>
        /// 保存會員信息
        /// </summary>
        /// <param name="memberopt"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult SaveMember([FromForm] MemberDto member)
        {
            SystemResult result = new SystemResult();
            try
            {
                //IMemberBLL bll = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();

                memberBLL.UpdateMember(member);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 通過id激活會員
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ActiveMember(string ids)
        {
            string[] idArray = ids.Split(',');
            SystemResult result = new SystemResult();
            try
            {
                //IMemberBLL bll = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();

                await memberBLL.ActiveMember(idArray.Select(d => new Guid(d)).ToArray());
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 通過id恢復會員
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> Restore(string ids)
        {
            string[] idArray = ids.Split(',');
            SystemResult result = new SystemResult();
            try
            {

                await memberBLL.Restore(idArray.Select(d => new Guid(d)).ToArray());
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 通過id停用會員
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> EnableMember(string ids)
        {
            string[] idArray = ids.Split(',');
            SystemResult result = new SystemResult();
            try
            {
                await memberBLL.EnableMember(idArray.Select(d => new Guid(d)).ToArray());
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 通過id停用會員
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> SuspendMember(string ids)
        {
            string[] idArray = ids.Split(',');
            SystemResult result = new SystemResult();
            try
            {

                await memberBLL.SuspendMember(idArray.Select(d => new Guid(d)).ToArray());
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 通過id刪除會員
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> DeleteMember(string ids)
        {
            string[] idArray = ids.Split(',');
            SystemResult result = new SystemResult();
            try
            {
                //IMemberBLL bll = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();

                await memberBLL.DeleteMember(idArray.Select(d => new Guid(d)).ToArray());
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 移動會員到指定組別
        /// </summary>
        /// <param name="ids"></param>
        /// /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> MoveMember(string ids, Guid groupId)
        {
            string[] idArray = ids.Split(',');
            SystemResult result = new SystemResult();
            try
            {
                //IMemberBLL bll = BLLFactory.Create(CurrentWebStoreConfig).CreateMemberBLL();

                await memberBLL.MoveMember(idArray.Select(d => new Guid(d)).ToArray(), groupId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public List<MallFunHistoryView> GetMallFunHistory(FunCond cond)
        {
            try
            {

                return memberBLL.GetMallFunHistory(cond);

            }
            catch (Exception ex)
            {
                return new List<MallFunHistoryView>();
            }
        }
    }
}
