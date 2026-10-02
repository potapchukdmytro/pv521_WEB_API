using Quartz;

namespace PV521_BooksShop.Jobs
{
    public class LogsCleanerJob : IJob
    {
        public ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var root = Directory.GetCurrentDirectory();
            var folderPath = Path.Combine(root, "logs");

            var files = Directory.GetFiles(folderPath);
            
            foreach(var file in files)
            {
                var fileInfo = new FileInfo(file);
                if(fileInfo.CreationTimeUtc < DateTime.UtcNow.AddMinutes(-7))
                {
                    fileInfo.Delete();
                }
            }

            return ValueTask.CompletedTask;
        }
    }
}
