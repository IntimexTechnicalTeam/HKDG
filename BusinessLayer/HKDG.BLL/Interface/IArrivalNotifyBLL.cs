using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDG.BLL
{
    public interface IArrivalNotifyBLL:IDependency
    {
        Task<SystemResult> CheckExsitArrivalNotify(ArrivalNotifyCond notify);

        Task<SystemResult> CancelArrivalNotify(ArrivalNotifyCond notify);

        Task<SystemResult> AddArrivalNotify(ArrivalNotifyCond notify);
    }
}
