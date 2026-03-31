using Microsoft.Extensions.Logging;
using QM.Models.Entities;
using QM.Repository.Interface;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QM.Repository.Repository
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository? _instance;
        private static readonly object _instanceLock = new();
        private readonly ILogger<QuantityMeasurementCacheRepository>? _logger;

        private readonly List<QuantityMeasurementEntity> _cache;
        private readonly object _cacheLock = new();
        private readonly string _jsonFilePath = "CacheMeasurements.json";
        private readonly object _jsonFileLock = new();

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
            LoadFromJsonFile();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_cacheLock)
            {
                _cache.Add(entity);
                _logger?.LogDebug($"Entity saved to cache. Total count: {_cache.Count}");
            }
            SaveToJsonFile(entity);
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
            lock (_jsonFileLock)
            {
                File.WriteAllText(_jsonFilePath, "[]");
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

        private void SaveToJsonFile(QuantityMeasurementEntity entity)
        {
            try
            {
                lock (_jsonFileLock)
                {
                    List<QuantityMeasurementEntity> allData = new();

                    // Read existing data from file if it exists
                    if (File.Exists(_jsonFilePath))
                    {
                        string json = File.ReadAllText(_jsonFilePath);
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                WriteIndented = true
                            };
                            allData = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json, options) ?? new();
                        }
                    }

                    // Add the new entity
                    allData.Add(entity);

                    // Write updated data back to file
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };
                    string updatedJson = JsonSerializer.Serialize(allData, jsonOptions);
                    File.WriteAllText(_jsonFilePath, updatedJson);

                    _logger?.LogDebug($"Entity saved to JSON file: {_jsonFilePath}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error saving to JSON file: {ex.Message}");
            }
        }

        public List<QuantityMeasurementEntity> GetErroredMeasurements()
        {
            lock (_cacheLock)
            {
                return _cache.Where(e => e.HasError).ToList();
            }
        }

        public int GetCountByOperationType(string operationType)
        {
            lock (_cacheLock)
            {
                return _cache.Count(e => e.OperationType.Equals(operationType, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void LoadFromJsonFile()
        {
            try
            {
                if (File.Exists(_jsonFilePath))
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var data = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json, options);
                        if (data != null)
                        {
                            _cache.AddRange(data);
                            _logger?.LogInformation($"Loaded {_cache.Count} record(s) from cache file.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error loading from JSON file: {ex.Message}");
            }
        }
    }
}
