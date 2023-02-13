using CoffeeScaffolding.CoffeeScaffoldingData.EntityDomain;
using System.ComponentModel.DataAnnotations;

namespace CoffeeScaffolding.CoffeeScaffoldingData.Models
{
    public class SYS_USER:BaseTable
    {
        public long ID { get; set; }        
        public string? ACCOUNT { get; set; } 
        public string? PASSWORD { get; set; }
        public string? USER_NAME { get; set; }
        public string? USER_NAME_EN { get; set; }
        public string? USER_EMAIL { get; set; }
        public string? USER_PHONE { get; set; }
        public string? ADDRESS { get; set; }
        public string? ID_CARD { get; set; }
        public string? OPEN_ID { get; set; }
        public string? UNION_ID { get; set; }
    }
}
