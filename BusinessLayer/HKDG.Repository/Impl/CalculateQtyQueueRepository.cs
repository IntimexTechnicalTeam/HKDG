using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDG.Repository
{
    public class CalculateQtyQueueRepository : ICalculateQtyQueueRepository
    {
        public IBaseRepository baseRepository;

        public IServiceProvider service;

        public CalculateQtyQueueRepository(IServiceProvider services)
        {
            this.service = services;
            baseRepository = service.GetService(typeof(IBaseRepository)) as IBaseRepository;
        }

        public async Task<int> UpdateState(Guid Id)
        {
            string sql = $"update CalculateQtyQueues set State=2,UpdateDate=GETDATE() where Id = @Id and State=1";
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@Id", Value = Id });

            return await baseRepository.ExecuteSqlCommandAsync(sql, param.ToArray());
        }

    }
}
