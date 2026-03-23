using Microsoft.Extensions.Logging;
using QM.Models.Entities;
using QM.Repository.Interface;

namespace QM.Repository.Repository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository? _instance;
        private static readonly object _instanceLock = new();
        private readonly ILogger<QuantityMeasurementCacheRepository>? _logger;

        private readonly List<QuantityMeasurementEntity> _cache;
        private readonly object _cacheLock = new();

        public static QuantityMeasurementCacheRepository Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new QuantityMeasurementCacheRepository();
                    return _instance;
                }
            }
        }

        public QuantityMeasurementCacheRepository(ILogger<QuantityMeasurementCacheRepository>? logger = null)
        {
            _cache = new List<QuantityMeasurementEntity>();
            _logger = logger;
            _logger?.LogInformation("Cache Repository initialized");
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_cacheLock)
            {
                _cache.Add(entity);
                _logger?.LogDebug($"Entity saved to cache. Total count: {_cache.Count}");
            }
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            lock (_cacheLock)
            {
                return new List<QuantityMeasurementEntity>(_cache);
            }
        }

        public QuantityMeasurementEntity? FindById(Guid id)
        {
            lock (_cacheLock)
            {
                return _cache.FirstOrDefault(e => e.Id == id);
            }
        }

        public void Clear()
        {
            lock (_cacheLock)
            {
                _cache.Clear();
                _logger?.LogInformation("Cache cleared");
            }
        }

        public int GetTotalCount()
        {
            lock (_cacheLock)
            {
                return _cache.Count;
            }
        }

        public List<QuantityMeasurementEntity> GetByOperationType(string operationType)
        {
            lock (_cacheLock)
            {
                return _cache.Where(e => e.OperationType.Equals(operationType, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType)
        {
            lock (_cacheLock)
            {
                return _cache.Where(e => (e.Operand1?.Contains(measurementType) ?? false) ||
                                        (e.Result?.Contains(measurementType) ?? false)).ToList();
            }
        }
    }
}
