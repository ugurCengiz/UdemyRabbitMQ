// See https://aka.ms/new-console-template for more information
//Publisher
using System.Net.Http.Headers;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ohictzrn:SEm-Y_sHOYw0AQM7WxkF7zecjptRF2bi@cow.rmq2.cloudamqp.com/ohictzrn");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.QueueDeclare("hello-queue", true, false, false);

Enumerable.Range(1,50).ToList().ForEach(x =>
{
    string message = $"Message {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});



Console.ReadLine();


