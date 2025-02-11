using Quartz;
using Quartz.Spi;

namespace CoffeeScaffolding.CoffeeScaffoldingUtil.Quartz
{
    public class QuartzFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QuartzFactory> _logger;

        public QuartzFactory(IServiceProvider serviceProvider, ILogger<QuartzFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var r = _serviceProvider.GetService(bundle.JobDetail.JobType) as IMsgJob;
            if (r != null)
            {
                return r;
            }
            else
            {
                _logger.LogError("not Quartz job Create");
                throw new NotImplementedException();
            }
        }
        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }

    }

}