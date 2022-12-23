namespace HKDG.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class MemberGroupController: WebController
    {
        public IMemberGroupBLL MemberGroupBLL { get; set; }
        public MemberGroupController(IComponentContext services) : base(services)
        {
            MemberGroupBLL = Services.Resolve<IMemberGroupBLL>();
        }
        /// <summary>
        /// 搜尋父組別
        /// </summary>
        /// <param name="name"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [HttpGet]

        public async Task<List<MemberGroupDto>> SearchMemGroup(string name, int limit, int offset)
        {
            //SystemResult result = new SystemResult();
            //result.ReturnValue = MemberGroupBLL.Search(name);
            //result.Succeeded = true;
            return MemberGroupBLL.Search(name);
        }

        /// <summary>
        /// 搜尋所有組別
        /// </summary>
        /// <param name="name"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [HttpGet]

        public List<MemberGroupDto> SearchAllMbrGroup(string name, int limit, int offset)
        {
            List<MemberGroupDto> list = new List<MemberGroupDto>();
            //string systemLang = WSCookie.GetUserLanguage();
            list = MemberGroupBLL.GetAll();
            return list;
        }


        /// <summary>
        /// 根據會員id獲取組別信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public MemberGroupDto GetMemGroup(Guid id)
        {
            MemberGroupDto memg = new MemberGroupDto();

            //IMemberGroupBLL bll = BLLFactory.Create(CurrentWebStore).CreateMemberGroupBLL();

            memg = MemberGroupBLL.GetById(id);

            return memg;
        }

        [HttpGet]
        public List<MbrGroupSelect> GetBuyerGroupsSelect()
        {
            var result = MemberGroupBLL.GetBuyerGroupsSelect();
            return result;
        }

        /// <summary>
        /// 獲取組別下的子組別
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public MemberGroupDto GetMemberGroupItem(Guid id)
        {
            MemberGroupDto memg = new MemberGroupDto();

            //IMemberGroupBLL bll = BLLFactory.Create(CurrentWebStore).CreateMemberGroupBLL();

            memg = MemberGroupBLL.GetById(id);

            return memg;
        }

        /// <summary>
        /// 獲取父組別下的子組別
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public List<MemberGroupDto> GetSubMemberGroup(Guid id)
        {
            List<MemberGroupDto> submems = new List<MemberGroupDto>();

            //IMemberGroupBLL bll = BLLFactory.Create(CurrentWebStore).CreateMemberGroupBLL();

            submems = MemberGroupBLL.GetSubGroup(id);

            //submems.Add(submem);

            return submems;
        }

        [HttpPost]
        public SystemResult SaveMbrGroupDisc(MemberGroupDto mbr)
        {
            SystemResult result = new SystemResult();
            try
            {

                result = MemberGroupBLL.SaveDiscount(mbr.Id, mbr.Discount);


            }
            catch (BLException blex)
            {
                result.Succeeded = false;
                result.Message = blex.Message;
            }
            return result;

        }

        /// <summary>
        /// 保存會員組別
        /// </summary>
        /// <param name="memberObj"></param>
        /// <returns></returns>
        [HttpPost]

        public SystemResult SaveMemberGroup([FromForm]MemberGroupDto memberObj)
        {
            SystemResult result = new SystemResult();
            try
            {
                result = MemberGroupBLL.Save(memberObj);

            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 刪除組別
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpGet]

        public SystemResult Delete(string idList)
        {
            string[] ids = idList.Split(',');
            SystemResult result = new SystemResult();
            try
            {
                result = MemberGroupBLL.DeleteMainGroup(ids.Select(d => new Guid(d)).ToList());
            }
            catch (BLException blex)
            {
                result.Succeeded = false;
                result.Message = blex.Message;
            }
            catch (Exception ex)
            {
                //LogManager.SaveError(CurrentWebStore, this.GetType().FullName, ClassUtility.GetMethodName(), "", ex.Message);
            }


            return result;
        }
    }
}
