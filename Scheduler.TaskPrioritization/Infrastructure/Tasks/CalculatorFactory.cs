namespace Scheduler.TaskPrioritization.Infrastructure.Tasks
{
    using System.Diagnostics;

    public static class CalculatorFactory
    {
        public static CatalanNumbersCalculator StartNew(int startValue = 0, int endValue = 500)
        {
            var sw = Stopwatch.StartNew();

            var catalanNumbersCalculator = new CatalanNumbersCalculator()
            {
                StartValue = startValue,
                EndValue = endValue
            };

            catalanNumbersCalculator.Execute();

            sw.Stop();
            catalanNumbersCalculator.ElapsedTime = sw.Elapsed;

            return catalanNumbersCalculator;
        }
    }
}