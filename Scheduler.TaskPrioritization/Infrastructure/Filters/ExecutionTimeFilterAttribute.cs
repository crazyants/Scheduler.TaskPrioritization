namespace Scheduler.TaskPrioritization.Infrastructure.Filters
{
    using System.Diagnostics;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class ExecutionTimeFilterAttribute : ActionFilterAttribute
    {
        private const string StopwatchKey = "StopwatchFilter.Value";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.Properties[StopwatchKey] = Stopwatch.StartNew();

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[StopwatchKey];
            stopwatch.Stop();

            actionExecutedContext.Response.Headers.Add("Execution-Time", stopwatch.ElapsedMilliseconds.ToString());
        }
    }
}