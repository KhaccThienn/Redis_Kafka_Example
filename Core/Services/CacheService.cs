namespace Remake_Kafka_Example_01.Core.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService(IConfiguration configuration)
        {
            var redis            = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            _cacheDb             = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb?.StringGet(key);
            if (string.IsNullOrEmpty(value))
            {
                return default(T)!;
            }

            return JsonSerializer.Deserialize<T>(value)!;
        }
        public void AddToList<T>(string key, T value)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            _cacheDb.ListLeftPush(key, serializedValue);
        }

        public object RemoveData(string key)
        {
            var _exist = _cacheDb.KeyExists(key);

            if (_exist)
                return _cacheDb.KeyDelete(key);

            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset? expirationTime = null)
        {
            if (expirationTime.HasValue)
            {
                var expiryTime = expirationTime?.DateTime.Subtract(DateTime.Now);
                return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
            }
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value));
        }
    }
}
