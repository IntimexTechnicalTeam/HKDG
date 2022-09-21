using System.Threading;
using MemberInfo = HKDG.Domain.MemberInfo;

namespace HKDG.BLL
{
    /// <summary>
    /// 
    /// </summary>
    [Dependency]
    public class TestHandler : INotificationHandler<MemberCreate<MemberInfo>>
    {
        public Task Handle(MemberCreate<MemberInfo> notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

