using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PizzaMail;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        Console.WriteLine("HEJ");
        services.AddHostedService<RabbitMqInitializer>();
        services.AddHttpClient("MailerSend", c =>
        {
            c.BaseAddress = new Uri("https://api.mailersend.com/");
            c.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("MAILERSEND_API_KEY")}");
        });
    })
    .Build();

host.Run();