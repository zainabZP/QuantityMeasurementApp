using QM.Models.DTOs;
using QM.Models.Entities;
using QM.Repository.Interface;  

namespace QM.Repository.Repository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        // ── Singleton ──────────────────────────────────────────
        private static QuantityMeasurementCacheRepository? _instance;
        private static readonly object _instanceLock = new();

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

        // Private constructor — enforces singleton
        private QuantityMeasurementCacheRepository()
        {
            _cache = new List<QuantityMeasurementEntity>();
        }

        // ── In-memory cache ────────────────────────────────────
        private readonly List<QuantityMeasurementEntity> _cache;
        private readonly object _cacheLock = new();

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_cacheLock)
            {
                _cache.Add(entity);
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
            }
        }

        public int Count
        {
            get { lock (_cacheLock) { return _cache.Count; } }
        }
    }
}