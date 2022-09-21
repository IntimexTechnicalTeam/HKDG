using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDG.BLL
{
    public interface ICalculateProductQtyBll : IDependency
    {
        //Task<SystemResult> DealProductQty(Guid Id);

        /// <summary>
        /// 补偿回写Qty
        /// </summary>
        /// <returns></returns>
        Task<SystemResult> HandleQtyAsync();

        Task<SystemResult> CalculateQty(Guid Id);
    }
}
