using System;
using System.Runtime.Caching;

namespace Common
{
    /// <summary>
    /// Specifies the relative priority of items stored in the cache object.
    /// </summary>
    public enum CachePriority
    {
        Default,
        NotRemovable
    }

    /// <summary>
    /// Represents the type that implements an in-memory cache of items.
    /// </summary>
    public static class ItemCache
    {
        //add event for item removed callback

        private static CacheItemPolicy _policy;
        private static CacheEntryRemovedCallback _callback;

        /// <summary>
        /// Adds or updates a cache entry into the cache using the specified key and value and the specified details for how it is to be evicted.
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to add or get.</param>
        /// <param name="cacheItem">The data for the cache entry.</param>
        /// <param name="cacheItemPriority">A priority setting that is used to determine whether to evict a cache entry.</param>
        public static void AddOrUpdateCachedItem<T>(string cacheKeyName, T cacheItem,
            CachePriority cacheItemPriority = CachePriority.Default)
        {
            _callback = CachedItemRemovedCallback;
            CacheItemPriority priority;
            Enum.TryParse(cacheItemPriority.ToString(), true, out priority);
            _policy = new CacheItemPolicy
            {
                Priority = priority,
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.00),
                RemovedCallback = _callback
            };
            
            // Add inside cache 
            MemoryCache.Default.Set(cacheKeyName, cacheItem, _policy);
        }

        /// <summary>
        /// Adds or gets a cache entry into the cache using the specified key and value and the specified details for how it is to be evicted.
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to add or get.</param>
        /// <param name="cacheItem">The data for the cache entry.</param>
        /// <param name="cacheItemPriority">A priority setting that is used to determine whether to evict a cache entry.</param>
        /// <returns>A cache entry.</returns>
        public static T AddOrGetCachedItem<T>(string cacheKeyName, T cacheItem,
            CachePriority cacheItemPriority = CachePriority.Default)
        {
            var val = GetCachedItem<T>(cacheKeyName);

            if (val == null || val.Equals(default(T)))
                AddOrUpdateCachedItem(cacheKeyName, cacheItem, cacheItemPriority);
            return GetCachedItem<T>(cacheKeyName);
        }

        /// <summary>
        /// Returns an entry from the cache. 
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to get.</param>
        /// <returns>A cache entry.</returns>
        public static T GetCachedItem<T>(string cacheKeyName)
        {
            return (T)MemoryCache.Default.Get(cacheKeyName);
        }

        /// <summary>
        /// Removes a cache entry from the cache.
        /// </summary>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to remove.</param>
        public static void RemoveCachedItem(string cacheKeyName)
        {
            if (MemoryCache.Default.Contains(cacheKeyName))
            {
                MemoryCache.Default.Remove(cacheKeyName);
            }
        }

        private static void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            // Log these values from arguments list 
            var strLog = string.Format("Reason: {0} | Key - Name:{1} | Value - Object:{2}", arguments.RemovedReason,
                arguments.CacheItem.Key, arguments.CacheItem.Value);

        }
    }

    /// <summary>
    /// Represents the type that implements an in-memory cache of tasks.
    /// </summary>
    public static class TaskCache
    {
        //add event for item removed callback

        private static CacheItemPolicy _policy;
        private static CacheEntryRemovedCallback _callback;

        /// <summary>
        /// Adds or gets a cache entry into the cache using the specified key and value and the specified details for how it is to be evicted.
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to add or get.</param>
        /// <param name="cacheTask">The function used to retrieve the data for the cache entry.</param>
        /// <param name="cacheItemPriority">A priority setting that is used to determine whether to evict a cache entry.</param>
        /// <returns>A cache entry.</returns>
        public static T AddOrGetCachedItem<T>(string cacheKeyName, Func<T> cacheTask,
            CachePriority cacheItemPriority = CachePriority.Default)
        {
            var val = GetCachedItem<T>(cacheKeyName);

            if (val == null || val.Equals(default(T)))
                AddOrUpdateCachedItem(cacheKeyName, cacheTask, cacheItemPriority);
            return GetCachedItem<T>(cacheKeyName);
        }

        /// <summary>
        /// Adds or updates a cache entry into the cache using the specified key and value and the specified details for how it is to be evicted.
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to add or get.</param>
        /// <param name="cacheTask">The function used to retrieve the data for the cache entry.</param>
        /// <param name="cacheItemPriority">A priority setting that is used to determine whether to evict a cache entry.</param>
        public static void AddOrUpdateCachedItem<T>(string cacheKeyName, Func<T> cacheTask,
            CachePriority cacheItemPriority = CachePriority.Default)
        {
            _callback = CachedItemRemovedCallback;
            CacheItemPriority priority;
            Enum.TryParse(cacheItemPriority.ToString(), true, out priority);
            _policy = new CacheItemPolicy
            {
                Priority = priority,
                //AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.00),
                RemovedCallback = _callback
            };

            // Add inside cache 
            MemoryCache.Default.Set(cacheKeyName, cacheTask.Invoke(), _policy);
        }

        /// <summary>
        /// Returns an entry from the cache. 
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to get.</param>
        /// <returns>A cache entry.</returns>
        public static T GetCachedItem<T>(string cacheKeyName)
        {
            return (T)MemoryCache.Default.Get(cacheKeyName);
        }

        /// <summary>
        /// Removes a cache entry from the cache.
        /// </summary>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to remove.</param>
        public static void RemoveCachedItem(string cacheKeyName)
        {
            if (MemoryCache.Default.Contains(cacheKeyName))
            {
                MemoryCache.Default.Remove(cacheKeyName);
            }
        }
        private static void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            // Log these values from arguments list 
            var strLog = string.Format("Reason: {0} | Key - Name:{1} | Value - Object:{2}", arguments.RemovedReason,
                arguments.CacheItem.Key, arguments.CacheItem.Value);
        }
    }

    /// <summary>
    /// Extension methods for the type TaskCache.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Adds or gets a cache entry into the cache using the specified key and value and the specified details for how it is to be evicted.
        /// </summary>
        /// <typeparam name="T">The type of the object returned.</typeparam>
        /// <param name="cacheKeyName">A unique identifier for the cache entry to add or get.</param>
        /// <param name="cacheTask">The function used to retrieve the data for the cache entry.</param>
        /// <param name="cacheItemPriority">A priority setting that is used to determine whether to evict a cache entry.</param>
        /// <returns>A cache entry.</returns>
        public static T AddOrGetCacheItem<T>(this Func<T> cacheTask, string cacheKeyName, CachePriority cacheItemPriority = CachePriority.Default)
        {
            return TaskCache.AddOrGetCachedItem(cacheKeyName, cacheTask, cacheItemPriority);
        }
    }   
}