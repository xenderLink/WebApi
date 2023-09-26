using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, EventArgs) =>
{
    // byte[]
    var body = EventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"New product processing is initiated - {message}");
};

channel.BasicConsume("products", true, consumer);

Console.ReadKey();