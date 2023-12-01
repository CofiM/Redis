﻿using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForumApp.Cache
{
    public class CacheProvider : ICacheProvider
    {
        private IDistributedCache cache { get; set; }

        private IConnectionMultiplexer redis { get; set; }

        public CacheProvider(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            this.cache = cache;
            this.redis = redis;
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options) 
            where T : class
        {
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
        }

        public async Task<T> GetAsync<T>(string key) 
            where T : class
        {
            var cacheResult = await cache.GetStringAsync(key);

            if (cacheResult == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(cacheResult);
        }

        public async Task DeleteAsync(string key)
        {
            await cache.RemoveAsync(key);
        }

        public void DeleteFromHashSet(string hashSetKey, string key)
        {
            var db = redis.GetDatabase();
            db.HashDelete(hashSetKey, key);
        }


        public void SetInHashSet(string hashSetKey, string key, string value)
        {
            var db = redis.GetDatabase();
            db.HashSet(hashSetKey, new HashEntry[] { new HashEntry(key, value) });
        }

        public T GetFromHashSet<T>(string hashSetKey, string key)
        {
            var db = redis.GetDatabase();
            var hashEntires = db.HashGetAll(hashSetKey);

            var stringValue = hashEntires.Where(item => item.Name == key).FirstOrDefault().Value.ToString();

            if (stringValue == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(stringValue);
        }

        public List<T> GetAllFromHashSet<T>(string hashSetKey) where T : class
        {
            var db = redis.GetDatabase();
            var hashEntires = db.HashGetAll(hashSetKey);

            var result = new List<T>();
            foreach (var entry in hashEntires)
            {
                result.Add(JsonSerializer.Deserialize<T>(entry.Value.ToString()));
            }

            return result;
        }
    }
}
