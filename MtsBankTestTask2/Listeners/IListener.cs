namespace MtsBankTestTask2.Listeners
{
    public interface IListener
    {
        Task ListenAsync(CancellationToken cancellationToken);
    }
}