﻿namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Infrastructure;
    using Scheduler.TaskPrioritization.Infrastructure.Scheduler;
    using Scheduler.TaskPrioritization.Infrastructure.Tasks;
    using Scheduler.TaskPrioritization.Models;

    public class Scenario5ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario5/";

        private const int MAX_NUMBER_OF_CONCURRENT_BATCH_REQUESTS = 3;

        private static int NUMBER_OF_CONCURRENT_BATCH_REQUESTS = 0;

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
        public async Task<IHttpActionResult> Batch([FromBody] TaskRequest request)
        {
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (NUMBER_OF_CONCURRENT_BATCH_REQUESTS < MAX_NUMBER_OF_CONCURRENT_BATCH_REQUESTS)
                    {
                        Interlocked.Increment(ref NUMBER_OF_CONCURRENT_BATCH_REQUESTS);

                        if (NUMBER_OF_CONCURRENT_BATCH_REQUESTS < MAX_NUMBER_OF_CONCURRENT_BATCH_REQUESTS)
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

                            Interlocked.Decrement(ref NUMBER_OF_CONCURRENT_BATCH_REQUESTS);
                            break;
                        }
                        else
                        {
                            Interlocked.Decrement(ref NUMBER_OF_CONCURRENT_BATCH_REQUESTS);
                        }
                    }
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, PriorityScheduler.Lowest).ConfigureAwait(false);

            return this.Ok();
        }
    }
}