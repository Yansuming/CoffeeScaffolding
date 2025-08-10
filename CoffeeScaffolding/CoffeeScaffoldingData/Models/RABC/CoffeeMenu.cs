namespace CoffeeScaffoldingData.Models.RABC
{
    /// <summary>
    /// 系统菜单类
    /// </summary>
    public class CoffeeMenu
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 菜单名称-权限
        /// </summary>
        public string? NormalizedMenuName { get; set; }

        /// <summary>
        /// 菜单名称-显示
        /// </summary>
        public string? MenuName { get; set; }

        /// <summary>
        /// 父级菜单ID（根菜单为0或null）
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 菜单路径或路由
        /// </summary>
        public string? Route { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; } = true;
    }
}