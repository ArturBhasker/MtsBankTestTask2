using Microsoft.Extensions.Configuration;
using MtsBankTestTask2.CounterServices;

namespace MtsBankTestTask2.Listeners
{
    internal class CommonFileCommaCountListener : IListener
    {
        private readonly ICommonCounterService _commonCounterService;
        private readonly IConfiguration _configuration;

        private FileSystemWatcher Watcher { get; set; } = new(Directory.GetCurrentDirectory());

        public CommonFileCommaCountListener(
            ICommonCounterService commonCounterService,
            IConfiguration configuration)
        {
            _commonCounterService =
                commonCounterService ?? throw new ArgumentNullException(nameof(commonCounterService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task ListenAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            Watcher =
                new FileSystemWatcher(_configuration["PathToListen"]);

            Watcher.Created += async (s, e) => await OnCreated(s, e, this);
            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        private static async Task OnCreated(
            object sender,
            FileSystemEventArgs e,
            CommonFileCommaCountListener commonFileListener)
        {
            var usableExtensions = commonFileListener
                ._configuration
                .AsEnumerable()
                .Where(item  =>  
                    item.Key.StartsWith("FileFilterExtensions") 
                    && item.Value != null
                    )
                .Select(item => item.Value);



            if (usableExtensions.Contains(Path.GetExtension(e.Name)))
            {
                return;
            }

            using var textStream = File.OpenText(e.FullPath);

            var commas = commonFileListener._commonCounterService.CountSymbol(textStream, ',');

            Console.WriteLine($"There are {commas} commas in file {e.Name}");

            await using var fs = new FileStream(
                commonFileListener._configuration["OutputFile"],
                FileMode.Append,
                FileAccess.Write
            );

            await using var textWriter = new StreamWriter(fs);

            await textWriter.WriteLineAsync($"{e.Name} - Подсчёт запятых - {commas}");
        }
    }
}