using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CoffeeScaffolding.Controllers.Filters
{
    public class ActionLogFilter : IAsyncActionFilter
    {
        private readonly ILogger<ActionLogFilter> _logger;

        public ActionLogFilter(ILogger<ActionLogFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = Stopwatch.StartNew();
            var http = context.HttpContext;
            var action = context.ActionDescriptor.DisplayName ?? "unknown";
            _logger.LogInformation("yan Action starting {Action} {Method} {Path}", action, http.Request.Method, http.Request.Path);

            var executedContext = await next();

            sw.Stop();
            _logger.LogInformation("yan Action finished {Action} in {ElapsedMs} ms with {Status}", action, sw.ElapsedMilliseconds, executedContext.HttpContext.Response?.StatusCode);
        }
    }
}
