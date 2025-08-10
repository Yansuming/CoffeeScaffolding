namespace CoffeeScaffoldingData.Models.RABC
{
    public class CoffeeFunction
    {
        public long Id { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 功能按钮-权限
        /// </summary>        
        public string? NormalizedFunctionName { get; set; }
        
        /// <summary>
        /// 功能按钮-显示
        /// </summary>        
        public string? FunctionName { get; set; }
    }
}