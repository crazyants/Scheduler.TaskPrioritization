namespace Scheduler.TaskPrioritization.ApiControllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Scheduler.TaskPrioritization.Infrastructure.Tasks;
    using Scheduler.TaskPrioritization.Models;

    public class Scenario1ApiController : ApiController
    {
        private const string BaseRoute = "api/scenario1/";

        [HttpPost]
        [Route(BaseRoute +  "realtime")]
        public IHttpActionResult Realtime([FromBody] TaskRequest request)
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

            return this.Ok();
        }

        [HttpPost]
        [Route(BaseRoute + "batch")]
        public IHttpActionResult Batch([FromBody] TaskRequest request)
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

            return this.Ok();
        }
    }
}