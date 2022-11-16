namespace HKDG.BLL
{
    public interface IPromotionBLL:IDependency
    {
        /// <summary>
        ///  获取首页推广数据
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        Task<List<PromotionPage>> ShowPromotionList(PromotionCond cond);
    }
}
