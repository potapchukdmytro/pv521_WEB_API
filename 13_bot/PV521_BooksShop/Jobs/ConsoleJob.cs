using Quartz;

namespace PV521_BooksShop.Jobs
{
    public class ConsoleJob : IJob
    {
        public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            await Console.Out.WriteLineAsync($"[{DateTime.Now}] Console job executed.");
            Console.ResetColor();
        }
    }
}
