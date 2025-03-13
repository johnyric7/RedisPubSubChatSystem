using Microsoft.AspNetCore.SignalR;
using RedisPubSubChatSystem.Services;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace RedisPubSubChatSystem;

public class ChatHub : Hub
{
    private readonly IRedisService _redisService;
    private readonly IDynamoDbService _dynamoDbService;
    private static ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

    public ChatHub(IRedisService redisService, IDynamoDbService dynamoDbService)
    {
        _redisService = redisService;
        _dynamoDbService = dynamoDbService;
    }

    public override async Task OnConnectedAsync()
    {
        string userName = Context.GetHttpContext().Request.Query["userName"];
        if (!string.IsNullOrEmpty(userName))
        {
            _userConnections[userName] = Context.ConnectionId;
            await RetrieveOfflineMessages(userName);
        }

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        string userName = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (!string.IsNullOrEmpty(userName))
        {
            _userConnections.TryRemove(userName, out _);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string fromUser, string toUser, string message)
    {
        if (_userConnections.TryGetValue(toUser, out string connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", fromUser, message);
            await _redisService.PublishMessageAsync($"{fromUser} -> {toUser}: {message}");
        }
        else
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "User is not connected, but they will receive the message once they connect.");
            await _dynamoDbService.StoreOfflineMessageAsync(toUser, message);
        }
    }

    private async Task RetrieveOfflineMessages(string user)
    {
        if (string.IsNullOrEmpty(user))
            return;

        var offlineMessages = await _dynamoDbService.RetrieveOfflineMessagesAsync(user);

        foreach (var message in offlineMessages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "System", message);
        }

        await _dynamoDbService.DeleteOfflineMessagesAsync(user);
    }
}
