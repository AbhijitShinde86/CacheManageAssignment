
namespace CacheManSys
{
    public class CacheItem
    {
        public string Key { get; set; }
        public string Content { get; set; }
        public DateTime LastAccessed { get; set; }
        public DateTime Created { get; set; }
        public TimeSpan TTL { get; set; }

        public bool IsExpired()
        {
            return DateTime.UtcNow - Created > TTL;
        }
    }
}
