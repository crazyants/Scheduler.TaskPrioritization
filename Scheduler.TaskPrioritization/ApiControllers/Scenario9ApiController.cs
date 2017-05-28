namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Infrastructure;
    using Scheduler.TaskPrioritization.Infrastructure.Scheduler;
    using Scheduler.TaskPrioritization.Infrastructure.Tasks;
    using Scheduler.TaskPrioritization.Models;

    public class Scenario9ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario9/";

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
        public async Task<IHttpActionResult> Batch([FromBody] TaskRequest request)
        {
            await Task.Factory.StartNew(() => 
            {
                for (int i = 0; i < request.NumberOfIterations; i++)
                {
                    var calculator = new CatalanNumbersCalculator()
                    {
                        StartValue = request.StartValue,
                        EndValue = request.EndValue
                    };

                    calculator.Execute();
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, PriorityScheduler.Lowest).ConfigureAwait(false);

            return this.Ok();
        }
    }
}