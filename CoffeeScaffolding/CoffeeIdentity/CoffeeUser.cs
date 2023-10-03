using Microsoft.AspNetCore.Identity;

namespace CoffeeScaffolding.Identity
{
    public class CoffeeUser:IdentityUser<long>
    {
        public string? WechatOpenid { get; set; }
    }
}
