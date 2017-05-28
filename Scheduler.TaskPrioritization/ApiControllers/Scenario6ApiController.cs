namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Infrastructure;
    using Scheduler.TaskPrioritization.Models;
    using Scheduler.TaskPrioritization.Tasks;

    public class Scenario6ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario6/";

        [HttpPost]
        [Route(BaseRoute + "realtime")]
        public async Task<IHttpActionResult> Realtime([FromBody] TaskRequest request)
        {
            await Task.Factory.StartNew(() =>
            {
                Parallel.For(0, request.NumberOfIterations, new ParallelOptions() { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount) }, i =>
                {
                    var calculator = new CatalanNumbersCalculator()
                    {
                        StartValue = request.StartValue,
                        EndValue = request.EndValue
                    };

                    calculator.Execute();
                });
            }, CancellationToken.None, TaskCreationOptions.PreferFairness, PriorityScheduler.Highest).ConfigureAwait(false);

            return this.Ok();
        }

        [HttpPost]
        [Route(BaseRoute + "batch")]
        public IHttpActionResult Batch([FromBody] TaskRequest request)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            var thread = new Thread(() =>
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
            });

            thread.Start();
            thread.Join();

            return this.Ok();
        }
    }
}