using CoffeeScaffolding.CoffeeScaffoldingData;
using CoffeeScaffolding.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeScaffolding.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly CoffeeScaffoldingDBContext db;
        private readonly UserManager<CoffeeUser> userManager;

        public SampleController( CoffeeScaffoldingDBContext db, UserManager<CoffeeUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [HttpGet]
        public JsonResult Get()
        {
            using(db)
            {
                var result = db.SYS_USER.ToList();
                return new JsonResult(result);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUser(string Name)
        {
            var result = await userManager.FindByNameAsync(Name);
            if (result == null)
            {
                return new JsonResult("not found");
            }
            return new JsonResult(result.UserName);

        }
    }
}
