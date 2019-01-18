using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infotecs.RabbitMQClient.Messaging.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infotecs.RabbitMQClient.Messaging
{
    /// <summary>
    /// Получатель сообщения.
    /// </summary>
    /// <typeparam name="TMessage">Тип обработанного сообщения.</typeparam>
    /// <typeparam name="TRawMessage">Тип необработанного сообщения.(Тип входящего сообщения до десериализации).</typeparam>
    /// <typeparam name="TMessageHandler">Тип обработчика сообщений.</typeparam>
    public abstract class MessageReceiver<TMessage, TRawMessage, TMessageHandler> : IHostedService where TMessageHandler : IMessageHandler<TMessage>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<MessageReceiver<TMessage, TRawMessage, TMessageHandler>> logger;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceScopeFactory">Фабрика для создания экземпляров <see cref="IServiceScope"/>.</param>
        /// <param name="logger">Логирования.</param>
        protected MessageReceiver(IServiceScopeFactory serviceScopeFactory, ILogger<MessageReceiver<TMessage, TRawMessage, TMessageHandler>> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Запустить асинхронный процесс получения сообщений.
        /// </summary>
        /// <param name="cancellationToken">Токен аннулирования задачи.</param>
        /// <returns><see cref="Task"/> объект асинхронного процесса получения сообщений.</returns>
        public abstract Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Остановить асинхронный процесс получения сообщений.
        /// </summary>
        /// <param name="cancellationToken">Токен аннулирования задачи.</param>
        /// <returns><see cref="Task"/> объект асинхронного процесса получения сообщений.</returns>
        public abstract Task StopAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Обрабатывать сообщения.
        /// </summary>
        /// <param name="rawMessage">Сообщение.</param>
        /// <returns>Результат обработки. True, если успешно обработано.</returns>
        protected async Task<bool> HandleMessageAsync(TRawMessage rawMessage)
        {
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                try
                {
                    await HandleMessageAsync(rawMessage, serviceScope);
                    return true;
                }
                catch(Exception ex)
                {
                    await HandleExceptionAsync(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Обрабатывать исключение.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <returns>Результат обработки.</returns>
        protected virtual Task HandleExceptionAsync(Exception exception)
        {
            logger.LogError(exception, "Unhandled exception while handle message");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Обрабатывать сообщения.
        /// </summary>
        /// <param name="rawMessage">Сообщение.</param>
        /// <param name="serviceScope"><see cref="IServiceScope"/>.</param>
        /// <returns>Результат обработки.</returns>
        protected virtual async Task HandleMessageAsync(TRawMessage rawMessage, IServiceScope serviceScope)
        {
            logger.LogInformation("Start handle message from sender of the message.");

            var message = DeserializeMessage(rawMessage);

            logger.LogInformation($"Message is deserialized. Type message: {typeof(TRawMessage).Name}");
            
            var handler = ActivatorUtilities.CreateInstance<TMessageHandler>(serviceScope.ServiceProvider);
            await handler.HandleAsync(message);
        }

        /// <summary>
        /// Десериализовать сообщения.
        /// </summary>
        /// <param name="rawMessage">Входящие сообщение.</param>
        /// <returns>Объект <see cref="TMessage"/>.</returns>
        protected abstract TMessage DeserializeMessage(TRawMessage rawMessage);
    }
}