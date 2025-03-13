using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisPubSubChatSystem.Repositories;

public class RedisRepository : IRedisRepository
{
    private const string _channel = "chat_channel";
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;

    public RedisRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _subscriber = redis.GetSubscriber();
    }

    public async Task PublishMessageAsync(string message)
    {
        await _subscriber.PublishAsync(_channel, message);
    }

    public async Task SubscribeToChannelAsync(Action<string> messageHandler)
    {
        await _subscriber.SubscribeAsync(_channel, (channel, message) => messageHandler(message));
    }
}
