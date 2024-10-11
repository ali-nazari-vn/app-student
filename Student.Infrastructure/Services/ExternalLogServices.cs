using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace StudentApp.Infrastructure
{
    public class ExternalLogServices
    {
        public void ExternalLog(Exception exception)
        {
            var queueName = "log-queu";

            var message = GetAllExceptionMessages(exception);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            ConnectionFactory connection = new()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var channel = connection.CreateConnection();

            using var model = channel.CreateModel();
            model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            model.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
        }


        string GetAllExceptionMessages(Exception ex)
        {
            string messages = ex.Message;
            Exception innerEx = ex.InnerException;

            while (innerEx != null)
            {
                messages += " --> " + innerEx.Message;
                innerEx = innerEx.InnerException;
            }

            return messages;
        }
    }
}
