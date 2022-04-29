using Microsoft.Extensions.Configuration;
using MtsBankTestTask2.CheckServices;

namespace MtsBankTestTask2.Listeners
{
    internal class CssCheckFigureScopeListener : IListener
    {
        private readonly ICommonCheckService _cssCheckService;
        private readonly IConfiguration _configuration;

        private FileSystemWatcher Watcher { get; set; } = new(Directory.GetCurrentDirectory());

        public CssCheckFigureScopeListener(
            ICommonCheckService cssCheckService,
            IConfiguration configuration)
        {
            _cssCheckService = cssCheckService ?? throw new ArgumentNullException(nameof(cssCheckService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task ListenAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            Watcher =
                new FileSystemWatcher(_configuration["PathToListen"], $"*{_configuration["FileFilterExtensions:Css"]}");

            Watcher.Created += async (s, e) => await OnCreated(s, e, this);
            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        private static async Task OnCreated(
            object sender,
            FileSystemEventArgs e,
            CssCheckFigureScopeListener cssCheckFigureScopeListener)
        {
            using var textStream = File.OpenText(e.FullPath);

            var checkScopes = cssCheckFigureScopeListener._cssCheckService.CheckFigureScopes(textStream);

            Console.WriteLine($"Bool result of check scope in file {e.Name} is {checkScopes}");

            await using var fs = new FileStream(
                cssCheckFigureScopeListener._configuration["OutputFile"],
                FileMode.Append,
                FileAccess.Write
            );

            await using var textWriter = new StreamWriter(fs);

            await textWriter.WriteLineAsync($"{e.Name} - Проверка figure scopes - {checkScopes}");
        }
    }
}