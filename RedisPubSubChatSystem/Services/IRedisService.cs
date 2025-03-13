using System;
using System.Threading.Tasks;

namespace RedisPubSubChatSystem.Services;

public interface IRedisService
{
    Task PublishMessageAsync(string message);
    Task SubscribeToChannelAsync(string channel, Action<string> messageHandler);
}