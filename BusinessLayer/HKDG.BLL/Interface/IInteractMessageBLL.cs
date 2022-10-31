using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDG.BLL
{
    public interface IInteractMessageBLL: IDependency
    {
        Task<NoticeListDto> GetLatesNoticeAsync(int top = 5);
    }
}
