using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDynamoDbService
{
    Task StoreOfflineMessageAsync(string user, string message);
    Task<List<string>> RetrieveOfflineMessagesAsync(string user);
    Task DeleteOfflineMessagesAsync(string user);
}