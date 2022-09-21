using System.Threading;

namespace HKDG.BLL
{
    public class SendLowQtyEmailHandler : BaseBLL, INotificationHandler<LowQtyEmailRequest>
    {
        public IInventoryChangeNotifyBLL inventoryChangeNotifyBLL;

        public SendLowQtyEmailHandler(IServiceProvider services) : base(services)
        {
            inventoryChangeNotifyBLL = Services.Resolve<IInventoryChangeNotifyBLL>();
        }

        public async Task Handle(LowQtyEmailRequest notification, CancellationToken cancellationToken)
        {
            await inventoryChangeNotifyBLL.CheckAndNotifyAsync(notification.skuList);
            //return Task.CompletedTask;
        }
    }

}
