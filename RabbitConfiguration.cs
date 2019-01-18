namespace Infotecs.RabbitMQClient.Messaging.Rabbit
{
    /// <summary>
    /// Конфигурация RabbitMQ.
    /// </summary>
    public class RabbitConfiguration
    {
        /// <summary>
        /// Хост.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя очереди.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Имя обмена.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Ключ маршрутизации.
        /// </summary>
        public string Routingkey { get; set; } = string.Empty;
    }
}
