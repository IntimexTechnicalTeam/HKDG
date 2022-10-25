using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCache;

namespace HKDG.BLL
{
    public class PreHeatMchFeeChargeService : AbstractPreHeatService
    {
        public PreHeatMchFeeChargeService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="Id">MerchantId</param>
        /// <returns></returns>
        public override async Task<SystemResult> CreatePreHeat(Guid Id)
        {
            var result = new SystemResult();

            var query = await GetDataSourceAsync(Guid.Empty);

            string key = PreHotType.Hot_MerchantFreeCharge.ToString();

            var hotList = query.ToList();
            if (hotList != null && hotList.Any())
            {
                var cacheData = await RedisHelper.HGetAllAsync<HotMerchantFreeCharge>(key);
                var fields = cacheData.Values.Where(x => x.MchId == Id).Select(s => s.Id.ToString()).ToArray();

                //全部删除
                await RedisHelper.HDelAsync(key, fields);
                foreach (var item in hotList)
                {
                    await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
                }
            }
            return result;
        }

        public async Task<IQueryable<HotMerchantFreeCharge>> GetDataSourceAsync(Guid Id)
        {
            var query = baseRepository.GetList<MerchantFreeCharge>().Where(x => x.IsActive && !x.IsDeleted && x.ProductCode != "" && x.ShipCode != "")
                               .Select(s => new HotMerchantFreeCharge
                               {
                                   Id = s.Id,
                                   MchId = s.MerchantId,
                                   ProductCode = s.ProductCode,
                                   ShipCode = s.ShipCode
                               });

            if (Id != Guid.Empty)
                query = query.Where(x => x.MchId == Id);

            return query;

        }

        public async Task<SystemResult> SetDataToHashCache(IQueryable<HotMerchantFreeCharge> list)
        {
            var result = new SystemResult();
            string key = PreHotType.Hot_MerchantFreeCharge.ToString();

            var hotList = list.ToList();
            if (hotList != null && hotList.Any())
            {
                //全部删除
                await RedisHelper.DelAsync(key);
                foreach (var item in hotList)
                {
                    await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
                }
            }
            return result;
        }
    }
}
