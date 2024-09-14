
using CacheManSys;

var cache = new CacheService(3);

// Add 3 Cache keys
cache.Add("key1", "cacheData1", TimeSpan.FromMinutes(10));
cache.Add("key2", "cacheData2", TimeSpan.FromMinutes(5));
cache.Add("key3", "cacheData3", TimeSpan.FromMinutes(15));


// Get key1 cache data
Console.WriteLine("cacheData for key1: " + cache.Get("key1"));
// Output : "cacheData1"


// Now Add new Cache key - key4
cache.Add("key4", "cacheData4", TimeSpan.FromMinutes(10));
// Output After adding key4 : key2 cache will remove as per LRU policy, beacuse key3 added after key2 and key1 we access before adding new


// Get key2 cache data
Console.WriteLine("cacheData for key2 after eviction: " + cache.Get("key2"));
// Output : null 


// Invalidate a specific cache ( manually )
cache.Invalidate("key1");
// Output : key1 cache will remove


// Get key1 cache data
Console.WriteLine("cacheData for key1 after invalidation: " + cache.Get("key1"));
// Output : null 


Console.ReadLine();