namespace Scheduler.TaskPrioritization.Models
{
    public class TaskRequest
    {
        public int NumberOfIterations { get; set; }

        public int StartValue { get; set; }

        public int EndValue { get; set; }
    }
}