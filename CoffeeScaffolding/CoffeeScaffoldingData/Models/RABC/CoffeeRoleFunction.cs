using System.ComponentModel.DataAnnotations;

namespace CoffeeScaffoldingData.Models.RABC
{
    public class CoffeeRoleFunction
    {
        public long Id { get; set; }        
        public long RoleId { get; set; }        
        public long MenuId { get; set; }
        public string? NormalizedMenuName { get; set; }        
        public long FunctionId { get; set; }        
        public string? NormalizedFunctionName { get; set; }
    }
}