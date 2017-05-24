namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Models;
    using Scheduler.TaskPrioritization.Tasks;

    public class Scenario4ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario4/";

        [HttpPost]
        [Route(BaseRoute + "realtime")]
        public IHttpActionResult Realtime([FromBody] TaskRequest request)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Parallel.For(0, request.NumberOfIterations, new ParallelOptions() { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount) }, i =>
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

            for (int i = 0; i < request.NumberOfIterations; i++)
            {
                var calculator = new CatalanNumbersCalculator()
                {
                    StartValue = request.StartValue,
                    EndValue = request.EndValue
                };

                calculator.Execute();
            }

            return this.Ok();
        }
    }
}