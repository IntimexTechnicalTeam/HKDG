namespace HKDG.BLL
{
    public interface ICalculateQtyService : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <param name="MsgId">PushMessage.Id</param>
        /// <returns></returns>
        Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId);
    }
}
