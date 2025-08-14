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
                ComResp resp = new ComResp(FlagEnum.Fail, new { ErrorMsg = executedContext.Exception.Message });
                executedContext.Result = new JsonResult(resp);
                executedContext.HttpContext.Response.StatusCode = 500;
            }
            else
            {
                // 成功时统一包装返回结果（避免重复包装）
                var result = executedContext.Result;
                switch (result)
                {
                    case ObjectResult obj when obj.Value is ComResp:
                        break; // 已经是 ComResp
                    case JsonResult json when json.Value is ComResp:
                        break; // 已经是 ComResp
                    case ObjectResult obj:
                        executedContext.Result = new JsonResult(new ComResp(FlagEnum.Error, obj.Value ?? new object()));
                        break;
                    case JsonResult json:
                        executedContext.Result = new JsonResult(new ComResp(FlagEnum.Error, json.Value ?? new object()));
                        break;
                    case EmptyResult:
                        executedContext.Result = new JsonResult(new ComResp(FlagEnum.Error, new object()));
                        break;
                    case StatusCodeResult statusRes:
                        executedContext.Result = new JsonResult(new ComResp(FlagEnum.Error, new { statusRes.StatusCode }));
                        break;
                    default:
                    // 其他诸如 FileResult/ChallengeResult/PhysicalFileResult 等保持不变，用于处理文件返回值
                        break;
                }

                sw.Stop();
                _logger.LogInformation("Action finished {Action} in {ElapsedMs} ms with {Status}", action, sw.ElapsedMilliseconds, executedContext.HttpContext.Response?.StatusCode);
            }
        }
    }
}
