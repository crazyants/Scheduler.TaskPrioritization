namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Models;
    using Scheduler.TaskPrioritization.Tasks;

    public class Scenario2ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario2/";

        [HttpPost]
        [Route(BaseRoute + "realtime")]
        public IHttpActionResult Realtime([FromBody] TaskRequest request)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Parallel.For(0, request.NumberOfIterations, i =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;

                var calculator = new CatalanNumbersCalculator()
                {
                    StartValue = request.StartValue,
                    EndValue = request.EndValue
                };

                calculator.Execute();
            });

            return this.Ok();
        }

        [HttpPost]
        [Route(BaseRoute + "batch")]
        public IHttpActionResult Batch([FromBody] TaskRequest request)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            Parallel.For(0, request.NumberOfIterations, i =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                var calculator = new CatalanNumbersCalculator()
                {
                    StartValue = request.StartValue,
                    EndValue = request.EndValue
                };

                calculator.Execute();
            });

            return this.Ok();
        }
    }
}