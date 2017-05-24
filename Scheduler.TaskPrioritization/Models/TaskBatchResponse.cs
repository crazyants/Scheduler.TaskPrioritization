namespace Scheduler.TaskPrioritization.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TaskBatchResponse
    {
        public static TaskBatchResponse GetResponse(IEnumerable<TaskResponse> taskResponses)
        {
            var totalTime = new TimeSpan(taskResponses.Sum(x => x.ElapsedTime.Ticks));

            var taskBatchResponse = new TaskBatchResponse()
            {
                ElapsedTime = totalTime
            };

            return taskBatchResponse;
        }

        public TimeSpan ElapsedTime { get; set; }
    }
}