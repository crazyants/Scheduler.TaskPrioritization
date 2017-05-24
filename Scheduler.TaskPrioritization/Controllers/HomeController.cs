namespace Scheduler.TaskPrioritization.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly Dictionary<string, string> descriptions = new Dictionary<string, string>()
        {
            ["Scenario #1"] = "<span class='label label-danger'>SLOW</span> <br /> <br /> Processing logic of realtime requests and Batch requests is the same. <br/ > No prioritization. <br/ > Both use Parallel.For().",
            ["Scenario #2"] = "<span class='label label-danger'>SLOW</span> <br /> <br /> Realtime requests are processing with Highest priority. <br/ > Batch requests are processing with Lowest priority. <br/ > Both use Parallel.For().",
            ["Scenario #3"] = "<span class='label label-info'>AVERAGE</span> <br /> <br /> Realtime requests are processing with Highest priority. <br/ > Batch requests are processing with Lowest priority. <br/ > Only the Realtime requests use Parallel.For().",
            ["Scenario #4"] = "<span class='label label-info'>AVERAGE</span> <br /> <br /> Similar to Scenario #3. <br/ > Realtime requests are processing with Highest priority. <br/ > Batch requests are processing with Lowest priority. <br/ > Only the Realtime requests use Parallel.For(). <br /> The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine.",
            ["Scenario #5"] = "<span class='label label-success'>FASTEST</span> <br /> <br /> Similar to Scenario #4. <br/ > The PriorityScheduler is used to set priority over Task instead via the Thread. <br/ > Only the Realtime requests use Parallel.For().",
            ["Scenario #6"] = "<span class='label label-success'>FASTEST</span> <br /> <br /> Similar to Scenario #5. <br/ > The PriorityScheduler is used to set priority over Task instead via the Thread. <br/ > Only the Realtime requests use Parallel.For(). <br /> The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine.",
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