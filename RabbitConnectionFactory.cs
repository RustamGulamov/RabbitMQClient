using System;
using RabbitMQ.Client;

namespace Infotecs.RabbitMQClient.Messaging.Rabbit
{
    /// <summary>
    /// Фабрика соединений RabbitMQ.
    /// </summary>
    public static class RabbitConnectionFactory
    {
        /// <summary>
        /// Создать фабрику соединений RabbitMQ.
        /// Главная точка входа в RabbitMQ .NET AMQP API.
        /// </summary>
        /// <param name="configuration">Конфигурация RabbitMQ.</param>
        /// <returns>Фабрика соединений RabbitMQ.</returns>
        public static ConnectionFactory CreateFactory(RabbitConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return new ConnectionFactory
            {
                HostName = configuration.Host,
                UserName = configuration.UserName,
                Password = configuration.Password,
                DispatchConsumersAsync = true
            };
        }
    }
}