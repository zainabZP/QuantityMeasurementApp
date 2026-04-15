using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using QM.Repository.Interface;

namespace QM.Repository.Repository;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly IDistributedCache              _cache;
    private readonly ILogger<TokenBlacklistService> _logger;
    private const string Prefix = "blacklist:";

    public TokenBlacklistService(IDistributedCache cache, ILogger<TokenBlacklistService> logger)
    {
        _cache  = cache;
        _logger = logger;
    }

    public void Revoke(string token)
    {
        var key     = Prefix + token;
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };

        _cache.SetString(key, "revoked", options);
        _logger.LogInformation("Token revoked and stored in Redis.");
    }

    public bool IsRevoked(string token)
    {
        var key = Prefix + token;
        return _cache.GetString(key) != null;
    }
}