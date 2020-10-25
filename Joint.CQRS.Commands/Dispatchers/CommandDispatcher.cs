using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Joint.CQRS.Commands.Dispatchers
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public CommandDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task SendAsync<T>(T command) where T : class, ICommand
        {
            using var scope = _serviceFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }

        public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command)
        {
            using var scope = _serviceFactory.CreateScope();
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

            return await handler.HandleAsync((dynamic)command);
        }

        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : class, ICommand<TResult>
        {
            using var scope = _serviceFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();

            return await handler.HandleAsync(command);
        }

    }
}