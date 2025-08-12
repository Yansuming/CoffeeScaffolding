using System.Diagnostics;
using CoffeeScaffolding.Controllers.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace CoffeeScaffolding.Controllers.Filters
{
    public class ActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<ActionFilter> _logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = Stopwatch.StartNew();

            var http = context.HttpContext;
            var action = context.ActionDescriptor.DisplayName ?? "unknown";
            _logger.LogInformation("Action starting {Action} {Method} {Path}", action, http.Request.Method, http.Request.Path);
            var executedContext = await next();
            if (executedContext.Exception != null)
            {
                // 处理异常
                executedContext.ExceptionHandled = true; // 标记异常已处理
                sw.Stop();
                _logger.LogError(executedContext.Exception, "Action error {Action} in {ElapsedMs} ms", context.ActionDescriptor.DisplayName, sw.ElapsedMilliseconds);
                ComResp resp = new ComResp(FlagEnum.Error, new { ErrorMsg = executedContext.Exception.Message });
                executedContext.Result = new JsonResult(resp);
                executedContext.HttpContext.Response.StatusCode = 500; 
            }
            else
            {
                sw.Stop();
                _logger.LogInformation("Action finished {Action} in {ElapsedMs} ms with {Status}", action, sw.ElapsedMilliseconds, executedContext.HttpContext.Response?.StatusCode);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { 
            
        }
    }
}
