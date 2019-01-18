using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Infotecs.RabbitMQClient.Messaging.Interfaces;

namespace Infotecs.RabbitMQClient.Messaging.Rabbit
{
    /// <summary>
    /// Отправитель сообщения в RabbitMQ.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    public class RabbitMessageSender<TMessage> : IMessageSender<TMessage>
    {
        private readonly RabbitConfiguration configuration;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Конфигурации RabbitMQ.</param>
        public RabbitMessageSender(RabbitConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Отправить сообщение в RabbitMQ. (Добавить сообщение в очередь).
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="cancellationToken">Токен аннулирования. Не используется.</param>
        /// <returns><see cref="Task"/> объект асинхронного отправки сообщения.</returns>
        public Task SendMessageAsync(TMessage message, CancellationToken cancellationToken)
        {
            var factory = RabbitConnectionFactory.CreateFactory(configuration);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(configuration.ExchangeName, ExchangeType.Direct);
                channel.BasicPublish(configuration.ExchangeName, configuration.Routingkey, null, SerializeMessage(message));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Сериализовать сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Сериализованные данные.</returns>
        protected virtual byte[] SerializeMessage(TMessage message)
        {
            var json = JsonConvert.SerializeObject(message, Formatting.Indented);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}