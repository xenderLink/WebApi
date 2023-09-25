using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Store.Services;

public sealed class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        };

        var connnection = factory.CreateConnection();

        using var channel = connnection.CreateModel();

        channel.QueueDeclare("products", durable: true, exclusive: true);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish("", "products", body: body);

    }
}