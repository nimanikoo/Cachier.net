using StackExchange.Redis;
using WebApiCaching.Services.Contracts;

namespace WebApiCaching.Services;

public class CacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<CacheService> logger)
    : ICacheService
{
    private readonly IDatabase _cacheDb = connectionMultiplexer.GetDatabase();
    private readonly ISubscriber _subscriber = connectionMultiplexer.GetSubscriber();

    // String operations

    /// <summary>
    /// Sets a string value in the cache with an expiration time.
    /// </summary>
    /// <param name="key">The key under which the value is stored.</param>
    /// <param name="value">The string value to store.</param>
    /// <param name="expirationTime">The expiration time of the key.</param>
    /// <returns>True if the operation succeeded, otherwise false.</returns>
    public async Task<bool> SetStringAsync(string key, string value, DateTimeOffset expirationTime)
    {
        try
        {
            TimeSpan expireTime = expirationTime.DateTime.Subtract(DateTime.UtcNow);
            return await _cacheDb.StringSetAsync(key, value, expireTime);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting string in cache. Key: {Key}", key);
            return false;
        }
    }

    /// <summary>
    /// Retrieves a string value from the cache by key.
    /// </summary>
    /// <param name="key">The key for which the value is retrieved.</param>
    /// <returns>The cached string value, or null if not found or an error occurs.</returns>
    public async Task<string> GetStringAsync(string key)
    {
        try
        {
            return await _cacheDb.StringGetAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting string from cache. Key: {Key}", key);
            return null;
        }
    }

    /// <summary>
    /// Removes a string value from the cache by key.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>True if the key was removed, otherwise false.</returns>
    public async Task<bool> RemoveStringAsync(string key)
    {
        try
        {
            return await _cacheDb.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing string from cache. Key: {Key}", key);
            return false;
        }
    }

    // Hash operations

    /// <summary>
    /// Sets a field in a hash stored in the cache.
    /// </summary>
    /// <param name="key">The hash key.</param>
    /// <param name="field">The field in the hash.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>True if the field was set, otherwise false.</returns>
    public async Task<bool> SetHashFieldAsync(string key, string field, string value)
    {
        try
        {
            return await _cacheDb.HashSetAsync(key, field, value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting hash field in cache. Key: {Key}, Field: {Field}", key, field);
            return false;
        }
    }

    /// <summary>
    /// Retrieves a field from a hash stored in the cache.
    /// </summary>
    /// <param name="key">The hash key.</param>
    /// <param name="field">The field in the hash.</param>
    /// <returns>The value of the field, or null if not found or an error occurs.</returns>
    public async Task<string> GetHashFieldAsync(string key, string field)
    {
        try
        {
            return await _cacheDb.HashGetAsync(key, field);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting hash field from cache. Key: {Key}, Field: {Field}", key, field);
            return null;
        }
    }

    /// <summary>
    /// Removes a field from a hash stored in the cache.
    /// </summary>
    /// <param name="key">The hash key.</param>
    /// <param name="field">The field in the hash.</param>
    /// <returns>True if the field was removed, otherwise false.</returns>
    public async Task<bool> RemoveHashFieldAsync(string key, string field)
    {
        try
        {
            return await _cacheDb.HashDeleteAsync(key, field);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing hash field from cache. Key: {Key}, Field: {Field}", key, field);
            return false;
        }
    }

    /// <summary>
    /// Retrieves all fields and values from a hash stored in the cache.
    /// </summary>
    /// <param name="key">The hash key.</param>
    /// <returns>A collection of key-value pairs representing the hash fields and values, or an empty collection if not found or an error occurs.</returns>
    public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllHashFieldsAsync(string key)
    {
        try
        {
            var hashEntries = await _cacheDb.HashGetAllAsync(key);
            return hashEntries.Select(entry => new KeyValuePair<string, string>(entry.Name, entry.Value));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all hash fields from cache. Key: {Key}", key);
            return Enumerable.Empty<KeyValuePair<string, string>>();
        }
    }

    // List operations

    /// <summary>
    /// Adds an item to a list in the cache.
    /// </summary>
    /// <param name="key">The list key.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>True if the item was added, otherwise false.</returns>
    public async Task<bool> AddToListAsync(string key, string value)
    {
        try
        {
            await _cacheDb.ListRightPushAsync(key, value);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding to list in cache. Key: {Key}", key);
            return false;
        }
    }

    /// <summary>
    /// Retrieves a range of items from a list in the cache.
    /// </summary>
    /// <param name="key">The list key.</param>
    /// <param name="start">The start index of the range.</param>
    /// <param name="stop">The end index of the range (inclusive).</param>
    /// <returns>A collection of items in the specified range, or an empty collection if not found or an error occurs.</returns>
    public async Task<IEnumerable<string>> GetListAsync(string key, long start = 0, long stop = -1)
    {
        try
        {
            var listItems = await _cacheDb.ListRangeAsync(key, start, stop);
            return listItems.Select(item => (string)item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting list from cache. Key: {Key}", key);
            return Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Removes an item from a list in the cache.
    /// </summary>
    /// <param name="key">The list key.</param>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if the item was removed, otherwise false.</returns>
    public async Task<bool> RemoveFromListAsync(string key, string value)
    {
        try
        {
            long removed = await _cacheDb.ListRemoveAsync(key, value);
            return removed > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing from list in cache. Key: {Key}", key);
            return false;
        }
    }

    // Set operations

    /// <summary>
    /// Adds an item to a set in the cache.
    /// </summary>
    /// <param name="key">The set key.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>True if the item was added, otherwise false.</returns>
    public async Task<bool> AddToSetAsync(string key, string value)
    {
        try
        {
            return await _cacheDb.SetAddAsync(key, value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding to set in cache. Key: {Key}", key);
            return false;
        }
    }

    /// <summary>
    /// Retrieves all members from a set in the cache.
    /// </summary>
    /// <param name="key">The set key.</param>
    /// <returns>A collection of set members, or an empty collection if not found or an error occurs.</returns>
    public async Task<IEnumerable<string>> GetSetMembersAsync(string key)
    {
        try
        {
            var members = await _cacheDb.SetMembersAsync(key);
            return members.Select(member => (string)member);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting set members from cache. Key: {Key}", key);
            return Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Removes an item from a set in the cache.
    /// </summary>
    /// <param name="key">The set key.</param>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if the item was removed, otherwise false.</returns>
    public async Task<bool> RemoveFromSetAsync(string key, string value)
    {
        try
        {
            return await _cacheDb.SetRemoveAsync(key, value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing from set in cache. Key: {Key}", key);
            return false;
        }
    }

    // Sorted Set operations

    /// <summary>
    /// Adds an item to a sorted set in the cache with a specified score.
    /// </summary>
    /// <param name="key">The sorted set key.</param>
    /// <param name="value">The value to add.</param>
    /// <param name="score">The score for the item.</param>
    /// <returns>True if the item was added, otherwise false.</returns>
    public async Task<bool> AddToSortedSetAsync(string key, string value, double score)
    {
        try
        {
            return await _cacheDb.SortedSetAddAsync(key, value, score);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding to sorted set in cache. Key: {Key}", key);
            return false;
        }
    }

    /// <summary>
    /// Retrieves a range of items from a sorted set in the cache by rank.
    /// </summary>
    /// <param name="key">The sorted set key.</param>
    /// <param name="start">The start rank of the range.</param>
    /// <param name="stop">The end rank of the range (inclusive).</param>
    /// <returns>A collection of sorted set members in the specified range, or an empty collection if not found or an error occurs.</returns>
    public async Task<IEnumerable<string>> GetSortedSetRangeAsync(string key, long start = 0, long stop = -1)
    {
        try
        {
            var members = await _cacheDb.SortedSetRangeByRankAsync(key, start, stop);
            return members.Select(member => (string)member);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting sorted set range from cache. Key: {Key}", key);
            return Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Removes an item from a sorted set in the cache.
    /// </summary>
    /// <param name="key">The sorted set key.</param>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if the item was removed, otherwise false.</returns>
    public async Task<bool> RemoveFromSortedSetAsync(string key, string value)
    {
        try
        {
            return await _cacheDb.SortedSetRemoveAsync(key, value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing from sorted set in cache. Key: {Key}", key);
            return false;
        }
    }

    // HyperLogLog operations

    /// <summary>
    /// Adds an item to a HyperLogLog in the cache for approximate cardinality estimation.
    /// </summary>
    /// <param name="key">The HyperLogLog key.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>True if the item was added, otherwise false.</returns>
    public async Task<bool> AddToHyperLogLogAsync(string key, string value)
    {
        try
        {
            return await _cacheDb.HyperLogLogAddAsync(key, value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding to HyperLogLog in cache. Key: {Key}", key);
            return false;
        }
    }

    /// <summary>
    /// Retrieves the approximate cardinality (number of unique elements) from a HyperLogLog in the cache.
    /// </summary>
    /// <param name="key">The HyperLogLog key.</param>
    /// <returns>The approximate number of unique elements, or 0 if an error occurs.</returns>
    public async Task<long> GetHyperLogLogCountAsync(string key)
    {
        try
        {
            return await _cacheDb.HyperLogLogLengthAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting HyperLogLog count from cache. Key: {Key}", key);
            return 0;
        }
    }

    // Transactions

    /// <summary>
    /// Executes a Redis transaction.
    /// </summary>
    /// <param name="transactionAction">A function that performs operations on the transaction.</param>
    /// <returns>True if the transaction was executed successfully, otherwise false.</returns>
    public async Task<bool> ExecuteTransactionAsync(Func<ITransaction, Task> transactionAction)
    {
        try
        {
            var transaction = _cacheDb.CreateTransaction();
            await transactionAction(transaction);
            return await transaction.ExecuteAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing Redis transaction.");
            return false;
        }
    }

    // Pub/Sub

    /// <summary>
    /// Subscribes to a Redis channel for receiving messages.
    /// </summary>
    /// <param name="channel">The channel to subscribe to.</param>
    /// <param name="onMessage">The action to execute when a message is received.</param>
    public async Task SubscribeAsync(string channel, Action<RedisChannel, RedisValue> onMessage)
    {
        try
        {
            await _subscriber.SubscribeAsync(channel, (ch, message) => onMessage(ch, message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error subscribing to Redis channel. Channel: {Channel}", channel);
        }
    }

    /// <summary>
    /// Publishes a message to a Redis channel.
    /// </summary>
    /// <param name="channel">The channel to publish to.</param>
    /// <param name="message">The message to publish.</param>
    public async Task PublishAsync(string channel, string message)
    {
        try
        {
            await _subscriber.PublishAsync(channel, message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing to Redis channel. Channel: {Channel}", channel);
        }
    }

    // Common operations

    /// <summary>
    /// Checks if a key exists in the cache.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key exists, otherwise false.</returns>
    public async Task<bool> KeyExistsAsync(string key)
    {
        try
        {
            return await _cacheDb.KeyExistsAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if key exists in cache. Key: {Key}", key);
            return false;
        }
    }

    /// <summary>
    /// Removes a key from the cache.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>True if the key was removed, otherwise false.</returns>
    public async Task<bool> RemoveDataAsync(string key)
    {
        try
        {
            return await _cacheDb.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing data from cache. Key: {Key}", key);
            return false;
        }
    }
}