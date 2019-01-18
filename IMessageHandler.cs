using System.Threading.Tasks;

namespace Infotecs.RabbitMQClient.Messaging.Interfaces
{
    /// <summary>
    /// Интерфейс обработчика сообщений.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    public interface IMessageHandler<in TMessage>
    {
        /// <summary>
        /// Обрабатывать сообщения.
        /// </summary>
        /// <param name="message">Cообщение.</param>
        /// <returns><see cref="Task"/> объект асинхронной обработки сообщения.</returns>
        Task HandleAsync(TMessage message);
    }
}