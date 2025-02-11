using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;

namespace CoffeeScaffolding.CoffeeScaffoldingUtil.Quartz
{
    public class QuartzSchedulerOperator
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <returns></returns>
        public static async Task<List<QuartzTaskInfo>> GetScheduler(ISchedulerFactory schedulerFactory, string jobGroup)
        {
            List<QuartzTaskInfo> taskInfos = new List<QuartzTaskInfo>();

            var schef = await schedulerFactory.GetScheduler();
            var groups = await schef.GetJobGroupNames();
            foreach (var group in groups)
            {
                var jobkeys = schef.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(jobGroup)).Result.ToList();
                //var tks = schef.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(jobGroup)).Result.ToList();
                //var tk = tks.Where(x => x.Name == jk.Name).FirstOrDefault();
                foreach (var jobkey in jobkeys)
                {
                    QuartzTaskInfo taskInfo = new QuartzTaskInfo();
                    var jobdetail = await schef.GetJobDetail(jobkey);
                    var trigger = schef.GetTriggersOfJob(jobkey).Result.FirstOrDefault();
                    if (jobdetail != null)
                    {
                        taskInfo.jobName = jobkey.Name;
                        taskInfo.jobGroup = jobkey.Group;
                        taskInfo.TaskTypeName = jobdetail.JobType.ToString();
                        taskInfo.triggerName = jobkey.Name;

                        if (trigger != null)
                        {
                            taskInfo.state = (int)schef.GetTriggerState(trigger.Key).Result;
                            if (trigger.GetPreviousFireTimeUtc() != null)
                            {
                                taskInfo.PreviousFireTime = Convert.ToDateTime(trigger.GetPreviousFireTimeUtc().ToString());
                            }
                            if (trigger.GetType().Name == nameof(SimpleTriggerImpl))
                            {
                                var ts = ((SimpleTriggerImpl)trigger).RepeatInterval;
                                taskInfo.seconds = ts.Seconds;
                            }
                            else if (trigger.GetType().Name == nameof(CronTriggerImpl))
                            {
                                var cron = ((CronTriggerImpl)trigger).CronExpressionString;
                                taskInfo.cron = cron;
                            }
                        }
                        taskInfos.Add(taskInfo);
                    }
                }
            }

            return taskInfos;
        }
        // add job to scheduler
        public static async Task<(bool, string)> AddJobtoScheduler(QuartzTaskInfo taskinfo, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            try
            {
                // get a scheduler
                var scheduler = await schedulerFactory.GetScheduler();

                // 获取group 并且判断 jobName 不存在时 增加 job
                List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(taskinfo.jobGroup)).Result.ToList();
                if (!jobKeys.Any(x => x.Name == taskinfo.jobName))
                {
                    scheduler.JobFactory = jobFactory;

                    // define the job and tie it to our demoJob class
                    Type? type = Type.GetType(taskinfo.TaskTypeName);
                    if (type != null)
                    {
                        IJobDetail job = JobBuilder.Create(type)
                            .WithIdentity(taskinfo.jobName, taskinfo.jobGroup)
                            .Build();
                        ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity(taskinfo.triggerName, taskinfo.jobGroup)
                            .StartAt(DateTimeOffset.Now.AddSeconds(3))
                            .WithCronSchedule(taskinfo.cron)
                        // .WithSimpleSchedule(x => x
                        //     .WithIntervalInSeconds(taskinfo.seconds)
                        //     .RepeatForever())
                        .Build();

                        await scheduler.ScheduleJob(job, trigger);
                        if (taskinfo.state == 0)
                        {
                            await scheduler.Start();
                        }
                        else
                        {
                            await PauseJobtoScheduler(taskinfo, schedulerFactory);
                        }
                    }
                    return (true, $"jobGroup:{taskinfo.jobGroup},jobName:{taskinfo.jobName} has been created successfully");
                }
                else
                {
                    return (false, $"jobGroup:{taskinfo.jobGroup},jobName:{taskinfo.jobName} already Exists");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        // update
        public static async Task<bool> ModifyJobtoScheduler(QuartzTaskInfo taskinfo, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(taskinfo.jobGroup)).Result.ToList();
            var jobKey = jobKeys.Where(x => x.Name == taskinfo.jobName).FirstOrDefault();
            if (jobKey != null)
            {
                await scheduler.DeleteJob(jobKey);
                await AddJobtoScheduler(taskinfo, schedulerFactory, jobFactory);
            }
            return true;
        }
        // delete
        public static async Task<bool> DeleteJobtoScheduler(QuartzTaskInfo taskinfo, ISchedulerFactory schedulerFactory)
        {
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(taskinfo.jobGroup)).Result.ToList();
            var jobKey = jobKeys.Where(x => x.Name == taskinfo.jobName).FirstOrDefault();
            if (jobKey != null)
            {
                await scheduler.DeleteJob(jobKey);
            }
            return true;
        }

        // start/resume
        public static async Task<bool> StartJobtoScheduler(QuartzTaskInfo taskinfo, ISchedulerFactory schedulerFactory)
        {
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(taskinfo.jobGroup)).Result.ToList();
            var jobKey = jobKeys.Where(x => x.Name == taskinfo.jobName).FirstOrDefault();
            if (jobKey != null)
            {
                await scheduler.ResumeJob(jobKey);
            }
            return true;
        }
        // pause
        public static async Task<bool> PauseJobtoScheduler(QuartzTaskInfo taskinfo, ISchedulerFactory schedulerFactory)
        {
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(taskinfo.jobGroup)).Result.ToList();
            var jobKey = jobKeys.Where(x => x.Name == taskinfo.jobName).FirstOrDefault();
            if (jobKey != null)
            {
                await scheduler.PauseJob(jobKey);
            }
            return true;
        }
    }

}