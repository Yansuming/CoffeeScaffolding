namespace CoffeeScaffolding.CoffeeScaffoldingUtil.Quartz
{
    public class QuartzTaskInfo
    {
        public string TaskTypeName { get; set; } = "";
        public string jobName { get; set; } = "";
        public string jobGroup { get; set; } = "";
        public string triggerName { get; set; } = "";
        public int seconds { get; set; } = 0;
        public string? cron { get; set; }
        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }
        /// <summary>
        /// 是否立即启动 0=立即启动； 1=不立即启动
        /// </summary>
        public int state { get; set; } = 0;
    }
}