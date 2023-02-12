using CoffeeScaffolding.CoffeeScaffoldingData;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeScaffolding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly CoffeeScaffoldingDBContext db;

        public SampleController( CoffeeScaffoldingDBContext db)
        {
            this.db = db;
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
    }
}
