using System.ComponentModel.DataAnnotations;

namespace CoffeeScaffolding.CoffeeScaffoldingData.Models
{
    public class SYS_USER
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
        public DateTime CREATE_TIME { get; set; }
        public long CREATE_USER_ID { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public long UPDATE_USER_ID { get; set;}
        public int DATA_VERSION { get; set; }
    }
}
