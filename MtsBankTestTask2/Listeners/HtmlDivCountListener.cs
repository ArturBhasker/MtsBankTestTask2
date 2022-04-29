using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using MtsBankTestTask2.CounterServices;

namespace MtsBankTestTask2.Listeners
{
    internal class HtmlDivCountListener : IListener
    {
        private readonly IHtmlCounterService _htmlCounterService;
        private readonly IConfiguration _configuration;

        private FileSystemWatcher Watcher { get; set; } = new(Directory.GetCurrentDirectory());

        public HtmlDivCountListener(
            IHtmlCounterService htmlCounterService,
            IConfiguration configuration)
        {
            _htmlCounterService = htmlCounterService ?? throw new ArgumentNullException(nameof(htmlCounterService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task ListenAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            Watcher =
                new FileSystemWatcher(_configuration["PathToListen"], $"*{_configuration["FileFilterExtensions:Html"]}");

            Watcher.Created += async (s, e) => await OnCreated(s, e, this);
            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        private static async Task OnCreated(
            object sender,
            FileSystemEventArgs e,
            HtmlDivCountListener htmlDivCountListener)
        {
            using var textStream = File.OpenText(e.FullPath);

            var hap = new HtmlDocument();
            hap.Load(textStream);

            var tagCount = htmlDivCountListener._htmlCounterService.CountTag(
                htmlDocument: hap,
                htmlTag: "//div");

            Console.WriteLine($"There are {tagCount} div tags in file {e.Name}");

            await using var fs = new FileStream(
                htmlDivCountListener._configuration["OutputFile"],
                FileMode.Append,
                FileAccess.Write
            );

            await using var textWriter = new StreamWriter(fs);

            await textWriter.WriteLineAsync($"{e.Name} - Подсчёт div - {tagCount}");
        }
    }
}