using CoffeeScaffolding.CoffeeScaffoldingData;
using CoffeeScaffolding.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoffeeScaffolding.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [ServiceFilter(typeof(CoffeeScaffolding.Filters.ActionLogFilter))]
    public class SampleController : ControllerBase
    {
        private readonly CoffeeScaffoldingDBContext db;
        private readonly UserManager<CoffeeUser> userManager;
        private readonly IMediator  mediator;

        public SampleController( CoffeeScaffoldingDBContext db, UserManager<CoffeeUser> userManager,IMediator  mediator)
        {
            this.db = db;
            this.userManager = userManager;
            this.mediator = mediator;
        }
        [HttpGet]
        public JsonResult Geterror()
        {
            int i = 0;
            int j = 1 / i;
            return new JsonResult("This j will not be reached due to the error above.");
        }

        [HttpGet]
        public JsonResult Get()
        {
            using(db)
            {
                var result = db.CoffeeUser.ToList();
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

        [HttpPost]
        public async Task<JsonResult> doMediatR()
        {
            doSometingBeforeSendMailMediatR ds = new doSometingBeforeSendMailMediatR("yansuming");            
            await mediator.Publish(ds);
            return new JsonResult("");
        }
    }
}
