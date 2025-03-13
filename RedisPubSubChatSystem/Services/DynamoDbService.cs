using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisPubSubChatSystem.Services;

public class DynamoDbService : IDynamoDbService
{
    private readonly IAmazonDynamoDB _dynamoDbClient;

    public DynamoDbService(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task StoreOfflineMessageAsync(string user, string message)
    {
        var table = Table.LoadTable(_dynamoDbClient, "OfflineMessages");
        var document = new Document
        {
            ["UserId"] = user,
            ["Timestamp"] = System.DateTime.UtcNow.ToString("o"),
            ["Message"] = message
        };
        await table.PutItemAsync(document);
    }

    public async Task<List<string>> RetrieveOfflineMessagesAsync(string user)
    {
        var table = Table.LoadTable(_dynamoDbClient, "OfflineMessages");
        var filter = new QueryFilter("UserId", QueryOperator.Equal, user);
        var search = table.Query(filter);
        var offlineMessages = new List<string>();

        while (!search.IsDone)
        {
            foreach (var document in await search.GetNextSetAsync())
            {
                offlineMessages.Add(document["Message"].AsString());
            }
        }

        return offlineMessages;
    }

    public async Task DeleteOfflineMessagesAsync(string user)
    {
        var table = Table.LoadTable(_dynamoDbClient, "OfflineMessages");
        var filter = new QueryFilter("UserId", QueryOperator.Equal, user);
        var search = table.Query(filter);

        while (!search.IsDone)
        {
            foreach (var document in await search.GetNextSetAsync())
            {
                await table.DeleteItemAsync(document);
            }
        }
    }
}
