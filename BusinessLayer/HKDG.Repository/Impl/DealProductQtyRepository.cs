namespace HKDG.Repository
{
    /// <summary>
    /// 这里只做数据同步，一切与Redis的数据为准，MQ从Redis中获取数据，回写数据库
    /// </summary>
    public class DealProductQtyRepository :  PublicBaseRepository ,IDealProductQtyRepository
    {    
        public DealProductQtyRepository(IServiceProvider service) : base(service)
        {
            
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="InvtHoldQty"></param>
        /// <param name="SalesQty"></param>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQty(int InvtActualQty, int InvtHoldQty, int SalesQty, int InvtReservedQty, Guid SkuId, Guid MsgId)
        {
            var result = new SystemResult();

            string sql = $@"update ProductQties set InvtActualQty =@InvtActualQty ,InvtHoldQty =@InvtHoldQty ,SalesQty=@SalesQty,InvtReservedQty=@InvtReservedQty,UpdateDate=GETDATE() where SkuId = @SkuId";
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@InvtActualQty", Value = InvtActualQty });
            param.Add(new SqlParameter { ParameterName = "@InvtHoldQty", Value = InvtHoldQty });
            param.Add(new SqlParameter { ParameterName = "@SalesQty", Value = SalesQty });
            param.Add(new SqlParameter { ParameterName = "@InvtReservedQty", Value = InvtReservedQty });
            param.Add(new SqlParameter { ParameterName = "@SkuId", Value = SkuId });

            string sql2 = $"update PushMessages set State =2 ,UpdateDate=GETDATE() where  Id = @Id";
            var param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter { ParameterName = "@Id", Value = MsgId });

            int flag = 0;
            using (var tran = baseRepository.CreateTransation())
            {
                flag += await baseRepository.ExecuteSqlCommandAsync(sql, param.ToArray());
                flag += await baseRepository.ExecuteSqlCommandAsync(sql2, param2.ToArray());
                tran.Commit();
            }

            result.Succeeded = flag > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 安排发货后，更新 InvtReservedQty，InvtActualQty，SalesQty
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="InvtHoldQty"></param>
        /// <param name="SalesQty"></param>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQty(int InvtActualQty, int SalesQty, int InvtReservedQty, Guid SkuId, Guid MsgId)
        {
            var result = new SystemResult();

            string sql = $@"update ProductQties set InvtActualQty =@InvtActualQty,SalesQty=@SalesQty,InvtReservedQty=@InvtReservedQty,UpdateDate=GETDATE() where SkuId = @SkuId";
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@InvtActualQty", Value = InvtActualQty });
            param.Add(new SqlParameter { ParameterName = "@SalesQty", Value = SalesQty });
            param.Add(new SqlParameter { ParameterName = "@InvtReservedQty", Value = InvtReservedQty });
            param.Add(new SqlParameter { ParameterName = "@SkuId", Value = SkuId });

            string sql2 = $"update PushMessages set State =2 ,UpdateDate=GETDATE() where  Id = @Id";
            var param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter { ParameterName = "@Id", Value = MsgId });

            int flag = 0;
            using (var tran = baseRepository.CreateTransation())
            {
                flag += await baseRepository.ExecuteSqlCommandAsync(sql, param.ToArray());
                flag += await baseRepository.ExecuteSqlCommandAsync(sql2, param2.ToArray());
                tran.Commit();
            }

            result.Succeeded = flag > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 创建订单,支付前取消，支付后到發貨前取消，支付超時或失败时，更新 InvtReservedQty
        /// </summary>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQty(int InvtReservedQty, Guid SkuId, Guid MsgId)
        {
            var result = new SystemResult();

            string sql = $@"update ProductQties set InvtReservedQty=@InvtReservedQty,UpdateDate=GETDATE() where SkuId = @SkuId";
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@InvtReservedQty", Value = InvtReservedQty });
            param.Add(new SqlParameter { ParameterName = "@SkuId", Value = SkuId });

            string sql2 = $"update PushMessages set State =2 ,UpdateDate=GETDATE() where  Id = @Id";
            var param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter { ParameterName = "@Id", Value = MsgId });

            int flag = 0;
            using (var tran = baseRepository.CreateTransation())
            {
                flag += await baseRepository.ExecuteSqlCommandAsync(sql, param.ToArray());
                flag += await baseRepository.ExecuteSqlCommandAsync(sql2, param2.ToArray());
                tran.Commit();
            }

            result.Succeeded = flag > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 采購入庫,采購退回,銷售退回，更新 InvtActualQty，SalesQty
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="SalesQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQty(int InvtActualQty, int SalesQty, Guid SkuId, Guid MsgId)
        {
            var result = new SystemResult();

            string sql = $@"update ProductQties set InvtActualQty =@InvtActualQty,SalesQty=@SalesQty,UpdateDate=GETDATE() where SkuId = @SkuId";
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@InvtActualQty", Value = InvtActualQty });
            param.Add(new SqlParameter { ParameterName = "@SalesQty", Value = SalesQty });
            param.Add(new SqlParameter { ParameterName = "@SkuId", Value = SkuId });

            string sql2 = $"update PushMessages set State =2 ,UpdateDate=GETDATE() where  Id = @Id";
            var param2 = new List<SqlParameter>();
            param2.Add(new SqlParameter { ParameterName = "@Id", Value = MsgId });

            int flag = 0;
            using (var tran = baseRepository.CreateTransation())
            {
                flag += await baseRepository.ExecuteSqlCommandAsync(sql, param.ToArray());
                flag += await baseRepository.ExecuteSqlCommandAsync(sql2, param2.ToArray());
                tran.Commit();
            }

            result.Succeeded = flag > 0 ? true : false;
            return result;
        }
    }
}
