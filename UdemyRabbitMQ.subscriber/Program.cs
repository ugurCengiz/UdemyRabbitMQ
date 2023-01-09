// See https://aka.ms/new-console-template for more information
//Subscriber
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ohictzrn:SEm-Y_sHOYw0AQM7WxkF7zecjptRF2bi@cow.rmq2.cloudamqp.com/ohictzrn");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// channel.QueueDeclare("hello-queue", true, false, false);

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("hello-queue",false,consumer);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);
    Console.WriteLine("Gelen Mesaj:" + message);
    channel.BasicAck(e.DeliveryTag,false);
};



Console.ReadLine();
