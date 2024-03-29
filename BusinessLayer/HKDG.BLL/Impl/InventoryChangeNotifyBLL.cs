﻿namespace HKDG.BLL
{
    public class InventoryChangeNotifyBLL : BaseBLL, IInventoryChangeNotifyBLL
    {
        public IProductRepository productRepository;

        public InventoryChangeNotifyBLL(IServiceProvider services) : base(services)
        {
            productRepository = Services.Resolve<IProductRepository>();
        }

        public SystemResult AddInventoryChangeNotify(InventoryChangeNotify notify)
        {
            var sysRslt = new SystemResult();

            if (notify != null)
            {
                //獲取未被處理的庫存變化通知
                var notifyRec = baseRepository.GetModel<InventoryChangeNotify>(x => x.SkuId == notify.SkuId && x.Type == notify.Type && !x.IsProcessed && x.IsActive && !x.IsDeleted);

                if (notifyRec == null)
                {
                    notifyRec = new InventoryChangeNotify()
                    {
                        Id = Guid.NewGuid(),
                        SkuId = notify.SkuId,
                        IsProcessed = false,
                        Type = notify.Type,
                        IsMerchNotified = true,
                        MerchNotifyDate = DateTime.Now,
                    };

                    //UnitOfWork.IsUnitSubmit = true;

                    baseRepository.Insert(notifyRec);

                    //UnitOfWork.Submit();

                    sysRslt.Succeeded = true;

                    //數據成功處理好，發出郵件
                    //SendNoticeToMerchant(notify);    //未完成
                }
                else
                {
                    if (!notifyRec.IsMerchNotified)
                    {
                        //UnitOfWork.IsUnitSubmit = true;

                        notifyRec.IsMerchNotified = true;
                        notifyRec.MerchNotifyDate = DateTime.Now;
                        baseRepository.Update(notifyRec);

                        //UnitOfWork.Submit();

                        sysRslt.Succeeded = true;

                        //數據成功處理好，發出郵件
                        //SendNoticeToMerchant(notify);    //未完成
                    }
                }
            }
            return sysRslt;
        }

        public async Task CheckAndNotifyAsync(IList<Guid> skuIds)
        {
            foreach (var skuId in skuIds)
            {
                //獲取商品的可銷售數量
                int saleableQty = (await baseRepository.GetModelAsync<ProductQty>(x => x.SkuId == skuId && x.IsActive && !x.IsDeleted))?.SalesQty ?? 0;
                var productSku = await baseRepository.GetModelAsync<ProductSku>(x => x.Id == skuId);
                string prodCode = productSku?.ProductCode;
                var lvProduct = productRepository.GetLastVersionProductByCode(prodCode);
                if (lvProduct != null)
                {
                    var product = await baseRepository.GetModelByIdAsync<Product>(lvProduct.Id);
                    var prduxtExt =await baseRepository.GetModelAsync<ProductExtension>(x => x.Id == product.Id && x.IsActive && !x.IsDeleted);
                    if (prduxtExt != null)
                    {
                        //低於安全庫存
                        if (saleableQty <= prduxtExt.SafetyStock)
                        {
                            InventoryChangeNotify notify = new InventoryChangeNotify()
                            {
                                SkuId = skuId,
                                Type = InvChangeNotifyType.LowThanSaftey,
                                //CurStockQty = saleableQty,
                            };
                            AddInventoryChangeNotify(notify);
                        }
                    }
                }

                if (saleableQty <= 0)
                {
                    //售罄
                    InventoryChangeNotify notify = new InventoryChangeNotify()
                    {
                        SkuId = skuId,
                        Type = InvChangeNotifyType.SoldOut,
                        //CurStockQty = saleableQty,
                    };
                    AddInventoryChangeNotify(notify);
                }
            }
        }
    }
}
