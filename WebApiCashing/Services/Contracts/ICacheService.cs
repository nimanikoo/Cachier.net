using StackExchange.Redis;

namespace WebApiCaching.Services.Contracts;

public interface ICacheService
{
    // String operations
    Task<bool> SetStringAsync(string key, string value, DateTimeOffset expirationTime);
    Task<string> GetStringAsync(string key);
    Task<bool> RemoveStringAsync(string key);

    // Hash operations
    Task<bool> SetHashFieldAsync(string key, string field, string value);
    Task<string> GetHashFieldAsync(string key, string field);
    Task<bool> RemoveHashFieldAsync(string key, string field);
    Task<IEnumerable<KeyValuePair<string, string>>> GetAllHashFieldsAsync(string key);

    // List operations
    Task<bool> AddToListAsync(string key, string value);
    Task<IEnumerable<string>> GetListAsync(string key, long start = 0, long stop = -1);
    Task<bool> RemoveFromListAsync(string key, string value);

    // Set operations
    Task<bool> AddToSetAsync(string key, string value);
    Task<IEnumerable<string>> GetSetMembersAsync(string key);
    Task<bool> RemoveFromSetAsync(string key, string value);

    // Sorted Set operations
    Task<bool> AddToSortedSetAsync(string key, string value, double score);
    Task<IEnumerable<string>> GetSortedSetRangeAsync(string key, long start = 0, long stop = -1);
    Task<bool> RemoveFromSortedSetAsync(string key, string value);

    // HyperLogLog operations
    Task<long> GetHyperLogLogCountAsync(string key);

    // Transactions
    Task<bool> ExecuteTransactionAsync(Func<ITransaction, Task> transactionAction);

    // Pub/Sub
    Task SubscribeAsync(string channel, Action<RedisChannel, RedisValue> onMessage);
    Task PublishAsync(string channel, string message);

    // Common operations
    Task<bool> KeyExistsAsync(string key);
    Task<bool> RemoveDataAsync(string key);
}