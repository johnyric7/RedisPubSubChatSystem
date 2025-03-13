using System;
using System.Threading.Tasks;
using RedisPubSubChatSystem.Repositories;

namespace RedisPubSubChatSystem.Services;

public class RedisService : IRedisService
{
    private readonly IRedisRepository _redisRepository;

    public RedisService(IRedisRepository redisRepository)
    {
        _redisRepository = redisRepository;
    }

    public async Task PublishMessageAsync(string message)
    {
        await _redisRepository.PublishMessageAsync(message);
    }

    public async Task SubscribeToChannelAsync(Action<string> messageHandler)
    {
        await _redisRepository.SubscribeToChannelAsync(messageHandler);
    }
}
