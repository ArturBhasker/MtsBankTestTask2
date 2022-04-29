using MtsBankTestTask2.Listeners;

namespace MtsBankTestTask2
{
    internal class Application
    {
        private readonly IEnumerable<IListener> _listeners;

        public Application(IEnumerable<IListener> listeners)
        {
            _listeners = listeners ?? throw new ArgumentNullException(nameof(listeners));
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var taskListeners = _listeners
                .Select(listener => listener.ListenAsync(cancellationToken));

            await Task.WhenAll(taskListeners);
        }
    }
}