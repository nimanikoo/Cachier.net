using Microsoft.AspNetCore.Mvc;
using WebApiCaching.Services.Contracts;

namespace WebApiCaching.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController(ICacheService cacheService) : ControllerBase
{
    #region String Operations

    /// <summary>
    /// Sets a string value in the cache with an expiration time.
    /// </summary>
    [HttpPost("set-string")]
    public async Task<IActionResult> SetStringAsync([FromQuery] string key, [FromQuery] string value,
        [FromQuery] DateTimeOffset expirationTime)
    {
        var result = await cacheService.SetStringAsync(key, value, expirationTime);
        return result ? Ok("String set successfully.") : BadRequest("Failed to set string.");
    }

    /// <summary>
    /// Retrieves a string value from the cache.
    /// </summary>
    [HttpGet("get-string")]
    public async Task<IActionResult> GetStringAsync([FromQuery] string key)
    {
        var value = await cacheService.GetStringAsync(key);
        return value != null ? Ok(value) : NotFound("String not found.");
    }

    /// <summary>
    /// Removes a string value from the cache.
    /// </summary>
    [HttpDelete("remove-string")]
    public async Task<IActionResult> RemoveStringAsync([FromQuery] string key)
    {
        var result = await cacheService.RemoveStringAsync(key);
        return result ? Ok("String removed successfully.") : BadRequest("Failed to remove string.");
    }

    #endregion

    #region Hash Operations

    /// <summary>
    /// Sets a field in a hash.
    /// </summary>
    [HttpPost("set-hash-field")]
    public async Task<IActionResult> SetHashFieldAsync([FromQuery] string key, [FromQuery] string field,
        [FromQuery] string value)
    {
        var result = await cacheService.SetHashFieldAsync(key, field, value);
        return result ? Ok("Hash field set successfully.") : BadRequest("Failed to set hash field.");
    }

    /// <summary>
    /// Retrieves a field from a hash.
    /// </summary>
    [HttpGet("get-hash-field")]
    public async Task<IActionResult> GetHashFieldAsync([FromQuery] string key, [FromQuery] string field)
    {
        var value = await cacheService.GetHashFieldAsync(key, field);
        return value != null ? Ok(value) : NotFound("Hash field not found.");
    }

    /// <summary>
    /// Removes a field from a hash.
    /// </summary>
    [HttpDelete("remove-hash-field")]
    public async Task<IActionResult> RemoveHashFieldAsync([FromQuery] string key, [FromQuery] string field)
    {
        var result = await cacheService.RemoveHashFieldAsync(key, field);
        return result ? Ok("Hash field removed successfully.") : BadRequest("Failed to remove hash field.");
    }

    /// <summary>
    /// Retrieves all fields and values from a hash.
    /// </summary>
    [HttpGet("get-all-hash-fields")]
    public async Task<IActionResult> GetAllHashFieldsAsync([FromQuery] string key)
    {
        var fields = await cacheService.GetAllHashFieldsAsync(key);
        return fields != null && fields.Any() ? Ok(fields) : NotFound("No hash fields found.");
    }

    #endregion

    #region List Operations

    /// <summary>
    /// Adds an item to a list.
    /// </summary>
    [HttpPost("add-to-list")]
    public async Task<IActionResult> AddToListAsync([FromQuery] string key, [FromQuery] string value)
    {
        var result = await cacheService.AddToListAsync(key, value);
        return result ? Ok("Item added to list.") : BadRequest("Failed to add item to list.");
    }

    /// <summary>
    /// Retrieves items from a list.
    /// </summary>
    [HttpGet("get-list")]
    public async Task<IActionResult> GetListAsync([FromQuery] string key, [FromQuery] long start = 0,
        [FromQuery] long stop = -1)
    {
        var items = await cacheService.GetListAsync(key, start, stop);
        return items != null && items.Any() ? Ok(items) : NotFound("List not found.");
    }

    /// <summary>
    /// Removes an item from a list.
    /// </summary>
    [HttpDelete("remove-from-list")]
    public async Task<IActionResult> RemoveFromListAsync([FromQuery] string key, [FromQuery] string value)
    {
        var result = await cacheService.RemoveFromListAsync(key, value);
        return result ? Ok("Item removed from list.") : BadRequest("Failed to remove item from list.");
    }

    #endregion

    #region Set Operations

    /// <summary>
    /// Adds an item to a set.
    /// </summary>
    [HttpPost("add-to-set")]
    public async Task<IActionResult> AddToSetAsync([FromQuery] string key, [FromQuery] string value)
    {
        var result = await cacheService.AddToSetAsync(key, value);
        return result ? Ok("Item added to set.") : BadRequest("Failed to add item to set.");
    }

    /// <summary>
    /// Retrieves members of a set.
    /// </summary>
    [HttpGet("get-set-members")]
    public async Task<IActionResult> GetSetMembersAsync([FromQuery] string key)
    {
        var members = await cacheService.GetSetMembersAsync(key);
        return members != null && members.Any() ? Ok(members) : NotFound("Set not found.");
    }

    /// <summary>
    /// Removes an item from a set.
    /// </summary>
    [HttpDelete("remove-from-set")]
    public async Task<IActionResult> RemoveFromSetAsync([FromQuery] string key, [FromQuery] string value)
    {
        var result = await cacheService.RemoveFromSetAsync(key, value);
        return result ? Ok("Item removed from set.") : BadRequest("Failed to remove item from set.");
    }

    #endregion

    #region Sorted Set Operations

    /// <summary>
    /// Adds an item to a sorted set with a score.
    /// </summary>
    [HttpPost("add-to-sorted-set")]
    public async Task<IActionResult> AddToSortedSetAsync([FromQuery] string key, [FromQuery] string value,
        [FromQuery] double score)
    {
        var result = await cacheService.AddToSortedSetAsync(key, value, score);
        return result ? Ok("Item added to sorted set.") : BadRequest("Failed to add item to sorted set.");
    }

    /// <summary>
    /// Retrieves items from a sorted set within a range.
    /// </summary>
    [HttpGet("get-sorted-set-range")]
    public async Task<IActionResult> GetSortedSetRangeAsync([FromQuery] string key, [FromQuery] long start = 0,
        [FromQuery] long stop = -1)
    {
        var items = await cacheService.GetSortedSetRangeAsync(key, start, stop);
        return items != null && items.Any() ? Ok(items) : NotFound("Sorted set not found.");
    }

    /// <summary>
    /// Removes an item from a sorted set.
    /// </summary>
    [HttpDelete("remove-from-sorted-set")]
    public async Task<IActionResult> RemoveFromSortedSetAsync([FromQuery] string key, [FromQuery] string value)
    {
        var result = await cacheService.RemoveFromSortedSetAsync(key, value);
        return result ? Ok("Item removed from sorted set.") : BadRequest("Failed to remove item from sorted set.");
    }

    #endregion

    #region HyperLogLog Operations

    /// <summary>
    /// Retrieves the count of unique items in a HyperLogLog structure.
    /// </summary>
    [HttpGet("get-hyperloglog-count")]
    public async Task<IActionResult> GetHyperLogLogCountAsync([FromQuery] string key)
    {
        var count = await cacheService.GetHyperLogLogCountAsync(key);
        return Ok(count);
    }

    #endregion

    #region Transactions

    /// <summary>
    /// Executes a Redis transaction.
    /// </summary>
    [HttpPost("execute-transaction")]
    public async Task<IActionResult> ExecuteTransactionAsync([FromQuery] string key, [FromQuery] string value)
    {
        var result = await cacheService.ExecuteTransactionAsync(transactionAction: async transaction =>
        {
            transaction.StringSetAsync(key, value);
            transaction.KeyDeleteAsync(key);
            await Task.CompletedTask;
        });

        return result ? Ok("Transaction executed successfully.") : BadRequest("Failed to execute transaction.");
    }

    #endregion

    #region Pub/Sub

    /// <summary>
    /// Subscribes to a Redis channel.
    /// </summary>
    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribeAsync([FromQuery] string channel)
    {
        await cacheService.SubscribeAsync(channel, (ch, message) =>
        {
            // Handle message here
            Console.WriteLine($"Message received from channel {ch}: {message}");
        });

        return Ok("Subscribed to channel.");
    }

    /// <summary>
    /// Publishes a message to a Redis channel.
    /// </summary>
    [HttpPost("publish")]
    public async Task<IActionResult> PublishAsync([FromQuery] string channel, [FromQuery] string message)
    {
        await cacheService.PublishAsync(channel, message);
        return Ok("Message published to channel.");
    }

    #endregion
}