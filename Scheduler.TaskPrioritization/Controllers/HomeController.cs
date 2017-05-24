namespace Scheduler.TaskPrioritization.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly Dictionary<string, string> descriptions = new Dictionary<string, string>()
        {
            ["Scenario #1"] = "Processing logic of realtime requests and Batch requests is the same. <br/ > No prioritization. <br/ > Both use Parallel.For().",
            ["Scenario #2"] = "Realtime requests are processing with Highest priority. <br/ > Batch requests are processing with Lowest priority. <br/ > Both use Parallel.For().",
            ["Scenario #3"] = "Realtime requests are processing with Highest priority. <br/ > Batch requests are processing with Lowest priority. <br/ > Only the Realtime requests use Paralle.For().",
            ["Scenario #4"] = "This is similar to Scenario #3. <br/ > The PriorityScheduler is used to set priority over Task instead via the Thread.",
        };

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Scenario(int id)
        {
            this.ViewBag.Title = $"Scenario #{id}";
            this.ViewBag.ApiControllerName = $"Scenario{id}";

            string scenarioDescription;
            this.descriptions.TryGetValue(this.ViewBag.Title, out scenarioDescription);
            this.ViewBag.Description = scenarioDescription;

            return this.View();
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var keys = this.descriptions.Keys.ToArray();
            var scenarioNames = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < keys.Length; i++)
            {
                scenarioNames.Add(new KeyValuePair<int, string>(i + 1, keys[i]));
            }

            this.ViewBag.ScenarioNames = scenarioNames;

            base.OnResultExecuting(filterContext);
        }
    }
}