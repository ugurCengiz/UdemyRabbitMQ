// See https://aka.ms/new-console-template for more information
//Subscriber
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Shared;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ohictzrn:SEm-Y_sHOYw0AQM7WxkF7zecjptRF2bi@cow.rmq2.cloudamqp.com/ohictzrn");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-mac","all");

channel.QueueBind(queueName,"header-exchange",String.Empty,headers);

channel.BasicConsume(queueName,false,consumer);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Product product = JsonSerializer.Deserialize<Product>(message);

    Thread.Sleep(1000);
    Console.WriteLine($"Gelen Mesaj:{product.Id}-{product.Name}-{product.Price}-{product.Stock}");

   

    channel.BasicAck(e.DeliveryTag,false);
};



Console.ReadLine();
