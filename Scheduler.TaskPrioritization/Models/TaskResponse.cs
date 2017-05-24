namespace Scheduler.TaskPrioritization.Models
{
    using System;
    using Scheduler.TaskPrioritization.Tasks;

    public class TaskResponse
    {
        public static TaskResponse GetResponse(CatalanNumbersCalculator calculator)
        {
            var taskResponse = new TaskResponse()
            {
                ElapsedTime = calculator.ElapsedTime,
                StartValue = calculator.StartValue,
                EndValue = calculator.EndValue
            };

            return taskResponse;
        }

        public TimeSpan ElapsedTime { get; set; }

        public int StartValue { get; set; }

        public int EndValue { get; set; }
    }
}