using System.Threading;
using System.Threading.Tasks;

namespace Infotecs.RabbitMQClient.Messaging.Interfaces
{
    /// <summary>
    /// Интерфейс отправителя сообщения.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    public interface IMessageSender<in TMessage>
    {
        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="cancellationToken">Токен аннулирования.</param>
        /// <returns><see cref="Task"/> объект асинхронного отправки сообщения.</returns>
        Task SendMessageAsync(TMessage message, CancellationToken cancellationToken);
    }
}