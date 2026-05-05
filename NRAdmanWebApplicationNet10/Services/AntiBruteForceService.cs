using Microsoft.Extensions.Caching.Memory;

namespace NRAdmanWebApplicationNet10.Services
{
    public interface IAntiBruteForceService
    {
        Task<bool> IsBlockedAsync(string ipAddress);
        Task RecordFailureAsync(string ipAddress);
        Task ResetFailuresAsync(string ipAddress);
    }

    public class AntiBruteForceService : IAntiBruteForceService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<AntiBruteForceService> _logger;
        private readonly int _maxAttempts;
        private readonly TimeSpan _window;
        private readonly TimeSpan _blockDuration;

        public AntiBruteForceService(IMemoryCache cache, IConfiguration configuration, ILogger<AntiBruteForceService> logger)
        {
            _cache = cache;
            _logger = logger;

            _maxAttempts = configuration.GetValue<int>("BruteForce:MaxAttemptsPerIp", 50);
            var windowMinutes = configuration.GetValue<int>("BruteForce:WindowMinutes", 60);
            var blockMinutes = configuration.GetValue<int>("BruteForce:BlockMinutes", 3600);

            _window = TimeSpan.FromMinutes(Math.Max(1, windowMinutes));
            _blockDuration = TimeSpan.FromMinutes(Math.Max(1, blockMinutes));
        }

        private static string CountKey(string ip) => $"bf:count:{ip}";
        private static string BlockKey(string ip) => $"bf:block:{ip}";

        public Task<bool> IsBlockedAsync(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress)) return Task.FromResult(false);
            var blocked = _cache.TryGetValue(BlockKey(ipAddress), out _);
            return Task.FromResult(blocked);
        }

        public Task ResetFailuresAsync(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress)) return Task.CompletedTask;
            _cache.Remove(CountKey(ipAddress));
            _cache.Remove(BlockKey(ipAddress));
            return Task.CompletedTask;
        }

        public Task RecordFailureAsync(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress)) return Task.CompletedTask;

            var countKey = CountKey(ipAddress);
            var blockKey = BlockKey(ipAddress);

            // If already blocked, nothing to do
            if (_cache.TryGetValue(blockKey, out _))
            {
                _logger.LogWarning("IP {Ip} attempted access while blocked", ipAddress);
                return Task.CompletedTask;
            }

            var count = 0;
            if (_cache.TryGetValue(countKey, out int existing))
            {
                count = existing;
            }
            count++;

            var entryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _window
            };

            _cache.Set(countKey, count, entryOptions);

            _logger.LogInformation("Recorded failed attempt {Count} for IP {Ip}", count, ipAddress);

            if (count >= _maxAttempts)
            {
                // Block the IP
                _cache.Set(blockKey, true, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _blockDuration
                });
                // Remove the counter
                _cache.Remove(countKey);
                _logger.LogWarning("IP {Ip} is blocked for {Minutes} minutes after {Attempts} failed attempts", ipAddress, _blockDuration.TotalMinutes, _maxAttempts);
            }

            return Task.CompletedTask;
        }
    }
}
