namespace OrderService.Infrastructure.Configuration;

public class CosmosDbOptions
{
    public string EndpointUri { get; set; } = string.Empty;
    public string PrimaryKey { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
