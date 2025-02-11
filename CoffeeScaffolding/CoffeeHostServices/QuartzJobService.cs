using CoffeeScaffolding.CoffeeScaffoldingUtil.Quartz;
using Quartz;

namespace CoffeeScaffolding.CoffeeHostServices
{
    [DisallowConcurrentExecution]
    public class QuartzJobService : IMsgJob
    {
        private ILogger<QuartzJobService> logger; 

        public QuartzJobService(ILogger<QuartzJobService> logger)
        {
            this.logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("执行开始");   
            
            logger.LogInformation("执行结束"); 
            return Task.CompletedTask;
        }
    }
}