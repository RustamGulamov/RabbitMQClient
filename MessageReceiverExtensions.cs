using System;
using Infotecs.RabbitMQClient.Messaging.Rabbit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infotecs.RabbitMQClient.Messaging.Extensions
{
    /// <summary>
    /// Расширения получателя сообщений.
    /// </summary>
    public static class MessageReceiverExtensions
    {
        /// <summary>
        /// Добавить получатель сообщения.
        /// </summary>
        /// <typeparam name="TReceiver">Тип получателя.</typeparam>
        /// <param name="serviceCollection">Коллекция сервисов.</param>
        /// <param name="factory">Фабрика для создания экземпляров <see cref="MessageReceiver{TMessage, TRawMessage, TMessageHandler}"/>.</param>
        public static void AddMessageReceiver<TReceiver>(this IServiceCollection serviceCollection, Func<IServiceProvider, IServiceScopeFactory, TReceiver> factory)
            where TReceiver : class, IHostedService
        {
            serviceCollection.AddSingleton<IHostedService, TReceiver>(serviceFactory => factory(serviceFactory, serviceFactory.GetRequiredService<IServiceScopeFactory>()));
        }

        /// <summary>
        /// Добавить получатель сообщения.
        /// </summary>
        /// <typeparam name="TReceiver">Тип получателя.</typeparam>
        /// <param name="serviceCollection">Коллекция сервисов.</param>
        /// <param name="rabbitConfiguration">Конфигурация RabbitMQ.</param>
        public static void AddMessageReceiver<TReceiver>(this IServiceCollection serviceCollection, RabbitConfiguration rabbitConfiguration) 
            where TReceiver : class, IHostedService
        {
            serviceCollection.AddMessageReceiver((serviceProvider, scopeFactory) => (TReceiver)Activator.CreateInstance(typeof(TReceiver), rabbitConfiguration, scopeFactory, serviceProvider.GetService<ILogger<TReceiver>>()));
        }
    }
}