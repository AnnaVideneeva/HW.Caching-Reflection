using System;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Caching
{
    /// <summary>
    /// Represents a service for caching.
    /// </summary>
    public class CachingService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingService"/> class.
        /// </summary>
        public CachingService()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _database = _redis.GetDatabase();
        }

        /// <summary>
        /// Set <paramref name="value"/> by <paramref name="key"/> to cache.
        /// </summary>
        /// <typeparam name="TObject">.</typeparam>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        public void SetToCache<TObject>(string key, TObject value)
        {
            var valueJson = this.Serialize(value);
            var expirationTimepiry = GetExpirationTime(value);
            this._database.StringSet(key, valueJson, expirationTimepiry);
        }

        /// <summary>
        /// Get value from cache by <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TObject">.</typeparam>
        /// <param name="key">A key for getting value.</param>
        /// <returns>Value from cahce with <paramref name="key"/>.</returns>
        public TObject GetFromCache<TObject>(string key)
        {
            var valueJson = this._database.StringGet(key);

            return valueJson.IsNullOrEmpty 
                ? default(TObject) 
                : Deserialize<TObject>(valueJson);
        }

        private string Serialize<TObject>(TObject obj) 
            => JsonConvert.SerializeObject(obj);

        private TObject Deserialize<TObject>(string str)
            => JsonConvert.DeserializeObject<TObject>(str);

        private TimeSpan? GetExpirationTime<T>(T value)
        {
            var cachingAttribute = (CachingAttribute)(typeof(T))
                .GetCustomAttributes(false)
                .FirstOrDefault(attribute => attribute.GetType() == typeof(CachingAttribute));

            return cachingAttribute?.ExpirationTime;
        }
    }
}