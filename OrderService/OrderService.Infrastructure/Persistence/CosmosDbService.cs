using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using OrderService.Infrastructure.Configuration;

namespace OrderService.Infrastructure.Persistence;

public class CosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _options;
    private Database? _database;
    private Container? _container;

    public CosmosDbService(IOptions<CosmosDbOptions> options)
    {
        _options = options.Value;

        var cosmosClientOptions = new CosmosClientOptions
        {
            HttpClientFactory = () => new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            })
            {
                Timeout = TimeSpan.FromSeconds(60)
            },
            ConnectionMode = ConnectionMode.Gateway,
            RequestTimeout = TimeSpan.FromSeconds(60)
        };

        _cosmosClient = new CosmosClient(_options.EndpointUri, _options.PrimaryKey, cosmosClientOptions);
    }

    public Container Container => _container ?? throw new InvalidOperationException("Container not initialized. Call InitializeDatabaseAsync first.");
    public Database Database => _database ?? throw new InvalidOperationException("Database not initialized. Call InitializeDatabaseAsync first.");

    public async Task InitializeDatabaseAsync()
    {
        var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_options.DatabaseName);
        _database = databaseResponse.Database;

        var containerResponse = await _database.CreateContainerIfNotExistsAsync(
            id: _options.ContainerName,
            partitionKeyPath: "/orderId",
            throughput: 400); 
        _container = containerResponse.Container;
    }
}
