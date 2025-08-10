namespace CoffeeScaffoldingData.Models.RABC
{
    // 用户与角色对应关系表
    public class CoffeeUserRole
    {        
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? UserName { get; set; }
        public long? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}