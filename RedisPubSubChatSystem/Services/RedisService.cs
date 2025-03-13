using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisPubSubChatSystem.Services;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _subscriber = redis.GetSubscriber();
    }

    public async Task PublishMessageAsync(string message)
    {
        await _subscriber.PublishAsync("chat_channel", message);
    }

    public async Task SubscribeToChannelAsync(string channel, Action<string> messageHandler)
    {
        await _subscriber.SubscribeAsync(channel, (channel, message) => messageHandler(message));
    }
}
