

namespace HandleCalculateQtyService
{
    public class HandleCalculateService : ConsumerHostServiceBase, IBackDoor
    {
        public HandleCalculateService(IServiceProvider services) : base(services)
        {
        }

        protected override string Queue => MQSetting.CalculateQtyQueue;

        protected override string Exchange => MQSetting.CalculateQtyExchange;

        protected override string categoryName => this.GetType().FullName;

        protected override async Task Handle(string msg)
        {
            using var scope = base.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<ICalculateProductQtyBll>();
            var result = await service.CalculateQty(Guid.Parse(msg));
            result.Message = result.Message ?? msg;
            SaveLog(result.Message, result.Succeeded);
        }
    }
}
