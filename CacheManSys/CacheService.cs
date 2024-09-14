namespace CacheManSys
{
    public class CacheService
    {
        private readonly int _capacity;
        private readonly Dictionary<string, LinkedListNode<CacheItem>> _cache;
        private readonly LinkedList<CacheItem> _cacheList;

        public CacheService(int capacity)
        {
            _capacity = capacity;
            _cache = new Dictionary<string, LinkedListNode<CacheItem>>(capacity);
            _cacheList = new LinkedList<CacheItem>();
        }

        public void Add(string key, string content, TimeSpan ttl)
        {
            var currentTime = DateTime.UtcNow;

            if (_cache.ContainsKey(key))
            {
                var node = _cache[key];
                node.Value.Content = content;
                node.Value.LastAccessed = currentTime;
                node.Value.Created = currentTime;
                node.Value.TTL = ttl;
                AddNode(node);
            }
            else
            {
                if (_cache.Count >= _capacity)
                {                    
                    RemoveLast(); // Remove LRU entry
                }

                var entry = new CacheItem
                {
                    Key = key,
                    Content = content,
                    LastAccessed = currentTime,
                    Created = currentTime,
                    TTL = ttl
                };
                var node = new LinkedListNode<CacheItem>(entry);
                _cacheList.AddFirst(node);
                _cache[key] = node;
            }
        }

        public string Get(string key)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                if (node.Value.IsExpired())
                {
                    Remove(key);
                    return null;
                }
                AddNode(node);
                return node.Value.Content;
            }
            return null; // Cache miss
        }

        public void Invalidate(string key)
        {
            Remove(key);
        }

        private void Remove(string key)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _cacheList.Remove(node);
                _cache.Remove(key);
            }
        }

        private void RemoveLast()
        {
            if (_cacheList.Count > 0)
            {
                var lruNode = _cacheList.Last;
                if (lruNode != null)
                {
                    _cache.Remove(lruNode.Value.Key);
                    _cacheList.RemoveLast();
                }
            }
        }

        private void AddNode(LinkedListNode<CacheItem> node)
        {
            _cacheList.Remove(node);
            _cacheList.AddFirst(node);
            node.Value.LastAccessed = DateTime.UtcNow;
        }
    }
}
