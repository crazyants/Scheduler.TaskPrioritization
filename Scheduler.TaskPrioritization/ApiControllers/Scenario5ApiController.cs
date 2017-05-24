﻿namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Infrastructure;
    using Scheduler.TaskPrioritization.Models;
    using Scheduler.TaskPrioritization.Tasks;

    public class Scenario5ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario5/";

        [HttpPost]
        [Route(BaseRoute + "realtime")]
        public async Task<IHttpActionResult> Realtime([FromBody] TaskRequest request)
        {
            await Task.Factory.StartNew(() =>
            {
                Parallel.For(0, request.NumberOfIterations, i =>
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