namespace Scheduler.TaskPrioritization.Infrastructure
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class PriorityScheduler : TaskScheduler
    {
        public static PriorityScheduler Highest = new PriorityScheduler(ThreadPriority.Highest);
        public static PriorityScheduler AboveNormal = new PriorityScheduler(ThreadPriority.AboveNormal);
        public static PriorityScheduler Normal = new PriorityScheduler(ThreadPriority.Normal);
        public static PriorityScheduler BelowNormal = new PriorityScheduler(ThreadPriority.BelowNormal);
        public static PriorityScheduler Lowest = new PriorityScheduler(ThreadPriority.Lowest);

        private readonly int maximumConcurrencyLevel = Math.Max(1, Environment.ProcessorCount);
        private readonly ThreadPriority priority;

        private readonly BlockingCollection<Task> tasks = new BlockingCollection<Task>();
        private Thread[] threads;

        public PriorityScheduler(ThreadPriority priority)
        {
            this.priority = priority;
        }

        public override int MaximumConcurrencyLevel => this.maximumConcurrencyLevel;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return this.tasks;
        }

        protected override void QueueTask(Task task)
        {
            this.tasks.Add(task);

            if (this.threads == null)
            {
                this.threads = new Thread[this.maximumConcurrencyLevel];

                for (var i = 0; i < this.threads.Length; i++)
                {
                    var thread = new Thread(() =>
                    {
                        foreach (var t in this.tasks.GetConsumingEnumerable())
                            this.TryExecuteTask(t);
                    })
                    {
                        Name = $"PriorityScheduler: {i}",
                        Priority = this.priority,
                        IsBackground = true
                    };

                    this.threads[i] = thread;
                    thread.Start();
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // We might not want to execute task that should schedule as high or low priority inline
            return false; 
        }
    }
}