using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisPubSubChatSystem.Repositories;

public interface IDynamoDbRepository
{
    Task StoreOfflineMessageAsync(string user, string message);
    Task<List<string>> RetrieveOfflineMessagesAsync(string user);
    Task DeleteOfflineMessagesAsync(string user);
}