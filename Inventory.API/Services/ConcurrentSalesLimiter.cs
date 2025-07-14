using Inventory.API.Exceptions;

namespace Inventory.API.Services
{
    public class ConcurrentSalesLimiter
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3, 3); // Limit to 3 concurrent

        public async Task<IDisposable> AcquireAsync(CancellationToken cancellationToken = default)
        {
            var acquired = await _semaphore.WaitAsync(TimeSpan.Zero, cancellationToken);
            if (!acquired)
            {
                throw new TooManyConcurrentSalesException();
            }

            return new SemaphoreRelease(_semaphore);
        }

        private class SemaphoreRelease : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;
            private bool _disposed;

            public SemaphoreRelease(SemaphoreSlim semaphore) => _semaphore = semaphore;

            public void Dispose()
            {
                if (_disposed) return;
                _semaphore.Release();
                _disposed = true;
            }
        }
    }
}
