using Microsoft.AspNetCore.Mvc;
using Model;

namespace HKDG.BLL.Impl
{
    public class ArrivalNotifyBLL : BaseBLL, IArrivalNotifyBLL
    {
        public ArrivalNotifyBLL(IServiceProvider services) : base(services)
        {
        }

        public async Task<SystemResult> CheckExsitArrivalNotify(ArrivalNotifyCond notify)
        {
            var result = new SystemResult();

            var prodSku = await baseRepository.GetModelAsync<ProductSku>(p => p.IsActive && !p.IsDeleted
                && p.ProductCode == notify.ProductCode && p.AttrValue1 == notify.AttrVal1 && p.AttrValue2 == notify.AttrVal2 && p.AttrValue3 == notify.AttrVal3);

            if (prodSku == null) throw new BLException(Resources.Message.SkuProductNotFound);
 
            var flag = await baseRepository.AnyAsync<Member>(x=>x.Id == CurrentUser.Id);
            var notifyList = await baseRepository.GetListAsync<ArrivalNotify>(x => x.IsActive && !x.IsDeleted && !x.IsNotified && x.NotifyDate == null && x.SkuId == prodSku.Id);

            if (flag) notifyList = notifyList.Where(x => x.MemberId == CurrentUser.Id);
            else notifyList = notifyList.Where(x => x.MemberId == null);

            if (notifyList != null && notifyList.Any())  result.Succeeded = true;

            return result;
        }

        /// <summary>
        /// 取消到货通知
        /// </summary>
        /// <param name="notify"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        public async Task<SystemResult> CancelArrivalNotify(ArrivalNotifyCond notify)
        {
            var result = new SystemResult();

            var prodSku = await baseRepository.GetModelAsync<ProductSku>(p => p.IsActive && !p.IsDeleted
                                    && p.ProductCode == notify.ProductCode && p.AttrValue1 == notify.AttrVal1 && p.AttrValue2 == notify.AttrVal2 && p.AttrValue3 == notify.AttrVal3);

            if (prodSku == null) throw new BLException(Message.SkuProductNotFound);
           
            UnitOfWork.IsUnitSubmit = true;

            var notifyList = await baseRepository.GetListAsync<ArrivalNotify>(x => x.IsActive && !x.IsDeleted && !x.IsNotified && x.NotifyDate == null && x.SkuId == prodSku.Id);
            if (notifyList!=null && notifyList.Any())
            {
                foreach (var notifyUp in notifyList)
                {
                    notifyUp.IsDeleted = true;
                }
                await baseRepository.UpdateAsync(notifyList);
                UnitOfWork.Submit();
            }

            result.Succeeded = true;
            result.Message = Message.CancelArrivalNotifySuccess;

            return result;
        }

        public async Task<SystemResult> AddArrivalNotify(ArrivalNotifyCond notify)
        {
            var result = new SystemResult();

            var prodSku = await baseRepository.GetModelAsync<ProductSku>(p => p.IsActive && !p.IsDeleted
                                      && p.ProductCode == notify.ProductCode && p.AttrValue1 == notify.AttrVal1 && p.AttrValue2 == notify.AttrVal2 && p.AttrValue3 == notify.AttrVal3);

            if (prodSku == null) return result;
            if (!prodSku.IsActive || prodSku.IsDeleted) throw new BLException(Resources.Message.SkuProductNotFound);

            var product = await baseRepository.GetModelAsync<Product>(x => x.IsApprove && x.Status == ProductStatus.OnSale && x.Code == prodSku.ProductCode);

            if (string.IsNullOrEmpty(notify.Email))
            {
                throw new Exception(Resources.Message.EmailNotNull);
            }

            var flag = await baseRepository.AnyAsync<Member>(x => x.Id == CurrentUser.Id);

            UnitOfWork.IsUnitSubmit = true;

            var notifyList = await baseRepository.GetListAsync<ArrivalNotify>(x => x.IsActive && !x.IsDeleted && !x.IsNotified && x.NotifyDate == null && x.SkuId == prodSku.Id);

            if (flag) notifyList = notifyList.Where(x => x.MemberId == CurrentUser.Id);
            else notifyList = notifyList.Where(x => x.MemberId == null && x.Email == notify.Email);

            ArrivalNotify currentNotify = null;

            if (notifyList!=null && notifyList.Any())
            {
                notifyList = notifyList.OrderByDescending(x => x.CreateDate);

                currentNotify = notifyList.FirstOrDefault();
                var delNotifyList = notifyList.Where(x => x.Id != currentNotify.Id).ToList();            
                foreach (var notifyUp in delNotifyList)
                {
                    notifyUp.IsDeleted = true;
                }
                await baseRepository.UpdateAsync(delNotifyList);
            }

            if (currentNotify == null)
            {
                currentNotify = new ArrivalNotify()
                {
                    Id = Guid.NewGuid(),
                    MemberId = CurrentUser.Id,
                    SkuId = prodSku.Id,
                    Email = notify.Email,
                    IsNotified = false,
                    NotifyDate = null,
                    IsMerchNotified = false,
                    MerchNotifyDate = null,
                    MerchantId = product.MerchantId,
                    IsActive = true,
                    IsDeleted = false,
                };
                await baseRepository.InsertAsync(currentNotify);
            }
            else
            {
                currentNotify.Email = notify.Email;
                await baseRepository.UpdateAsync(currentNotify);
            }

            //Remark，修改由Schedule Job定時統計後集中發送一次
            //SendReplenishmentAlertToMerchant(prodSku.ProductCode, notify.SkuId);

            UnitOfWork.Submit();

            result.Succeeded = true;
            result.Message = Message.AddArrivalNotifySuccess;
            result.ReturnValue = currentNotify.Id;
            return result;
        }
    }
}
