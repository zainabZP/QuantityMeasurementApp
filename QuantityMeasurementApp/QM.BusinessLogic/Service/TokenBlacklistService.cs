// namespace QM.BusinessLogic.Service;

// public interface ITokenBlacklistService
// {
//     void Revoke(string token);
//     bool IsRevoked(string token);
// }

// public class TokenBlacklistService : ITokenBlacklistService
// {
//     private readonly HashSet<string> _revoked = new();
//     private readonly object _lock = new();

//     public void Revoke(string token)
//     {
//         lock (_lock) _revoked.Add(token);
//     }

//     public bool IsRevoked(string token)
//     {
//         lock (_lock) return _revoked.Contains(token);
//     }
// }


// using Microsoft.Extensions.Caching.Distributed;
// using Microsoft.Extensions.Logging;
// using QM.BusinessLogic.Service;
// using QM.BusinessLogic.Interface;

// namespace QM.BusinessLogic.Service;

// public class TokenBlacklistService : ITokenBlacklistService
// {
//     private readonly IDistributedCache          _cache;
//     private readonly ILogger<TokenBlacklistService> _logger;
//     private const string Prefix = "blacklist:";

//     public TokenBlacklistService(IDistributedCache cache, ILogger<TokenBlacklistService> logger)
//     {
//         _cache  = cache;
//         _logger = logger;
//     }

//     public void Revoke(string token)
//     {
//         var key     = Prefix + token;
//         var options = new DistributedCacheEntryOptions
//         {
//             // Token stays blacklisted for 24 hours
//             AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
//         };

//         // Store synchronously since interface is not async
//         _cache.SetString(key, "revoked", options);
//         _logger.LogInformation("Token revoked and stored in Redis.");
//     }

//     public bool IsRevoked(string token)
//     {
//         var key    = Prefix + token;
//         var result = _cache.GetString(key);
//         return result != null;
//     }
// }