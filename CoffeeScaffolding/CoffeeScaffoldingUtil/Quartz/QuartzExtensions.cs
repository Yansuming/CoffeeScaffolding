using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;

namespace CoffeeScaffolding.CoffeeScaffoldingUtil.Quartz
{
    public static class QuartzExtensions
    {
        public static void AddQuartz(this IServiceCollection services)
        {
            //IOC
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, QuartzFactory>();

            // 获取程序集中，所有继承并且实现了IJob的类，并将他们注入到services
            var IJobInstanceLists = typeof(QuartzExtensions).Assembly.GetTypes().Where(t => !t.IsInterface && typeof(IMsgJob).IsAssignableFrom(t)).ToList();
            foreach (var IJobInstance in IJobInstanceLists)
            {
                if (!string.IsNullOrEmpty(IJobInstance.FullName))
                {
                    var JobInstance = Type.GetType(IJobInstance.FullName);
                    if (JobInstance != null)
                    {
                        services.AddSingleton(JobInstance);
                    }
                }
            }
        }

        public static IApplicationBuilder RunQuartzJob(this IApplicationBuilder app, string TransferCron)
        {
            //根据配置启动项,构建完整任务消息类            
            List<QuartzTaskInfo> QuartzTaskInfos = new List<QuartzTaskInfo>();

            QuartzTaskInfo qInfo = new QuartzTaskInfo();

            qInfo.TaskTypeName = $"CoffeeScaffolding.CoffeeHostServices.QuartzJobService";
            qInfo.jobName = $"QuartzJobService";
            qInfo.jobGroup = "CoffeeGroup";
            qInfo.triggerName = $"CoffeeTrigger";
            qInfo.state = 0;
            qInfo.cron = TransferCron; //Cron表达式

            QuartzTaskInfos.Add(qInfo);            
            IServiceProvider services = app.ApplicationServices;
            var _schedulerFactory = services.GetService<ISchedulerFactory>();
            var _jobFactory = services.GetService<IJobFactory>();
            if (_schedulerFactory != null && _jobFactory != null)
            {
                foreach (var taskinfo in QuartzTaskInfos)
                {
                    QuartzSchedulerOperator.AddJobtoScheduler(taskinfo, _schedulerFactory, _jobFactory).GetAwaiter().GetResult();
                }
            }

            return app;
        }
    }

}