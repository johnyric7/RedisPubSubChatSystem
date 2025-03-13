using System;
using System.Threading.Tasks;

namespace RedisPubSubChatSystem.Repositories;

public interface IRedisRepository
{
    Task PublishMessageAsync(string message);
    Task SubscribeToChannelAsync(Action<string> messageHandler);
}