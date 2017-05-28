namespace Scheduler.TaskPrioritization.Infrastructure.Tasks
{
    using System;
    using System.Numerics;

    public class CatalanNumbersCalculator
    {
        public int StartValue { get; set; }

        public int EndValue { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public void Execute()
        {
            for (var i = this.StartValue; i <= this.EndValue; i++)
            {
                var catalanNumber = this.Factorial(2 * i) / (this.Factorial(i + 1) * this.Factorial(i));
            }
        }

        private BigInteger Factorial(int n)
        {
            if (n < 1)
            {
                return 1;
            }

            return n * this.Factorial(n - 1);
        }
    }
}