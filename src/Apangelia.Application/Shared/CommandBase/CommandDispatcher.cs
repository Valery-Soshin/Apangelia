using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Apangelia.Application.Shared.CommandBase;

/// <summary>
/// Стандартный отправитель команд приложения через контейнер зависимостей.
/// </summary>
public sealed class CommandDispatcher : ICommandDispatcher
{
    private static readonly MethodInfo HandleCoreAsyncMethod = typeof(CommandDispatcher).GetMethod(
        nameof(HandleCoreAsync),
        BindingFlags.Instance | BindingFlags.NonPublic)!;

    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> HandleAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();
        var handleMethod = HandleCoreAsyncMethod.MakeGenericMethod(commandType, typeof(TResult));

        try
        {
            return (Task<TResult>)handleMethod.Invoke(this, [command, cancellationToken])!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            // Reflection заворачивает исключение обработчика; перекидываем исходное без потери stack trace.
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

    private Task<TResult> HandleCoreAsync<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        var handler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResult>));

        if (handler is not ICommandHandler<TCommand, TResult> commandHandler)
        {
            throw new InvalidOperationException(
                $"Command handler for command type '{typeof(TCommand).FullName}' and result type '{typeof(TResult).FullName}' is not registered.");
        }

        var behaviors = _serviceProvider.GetService(typeof(IEnumerable<IPipelineBehavior<TCommand, TResult>>));
        var pipelineBehaviors = behaviors is IEnumerable<IPipelineBehavior<TCommand, TResult>> typedBehaviors
            ? typedBehaviors
            : [];

        // Начинаем с реального обработчика и оборачиваем его аспектами от внутренних к внешним.
        CommandHandlerDelegate<TResult> next = () => commandHandler.HandleAsync(command, cancellationToken);

        foreach (var behavior in pipelineBehaviors.Reverse())
        {
            // Reverse сохраняет порядок DI: первый зарегистрированный behavior станет внешним в цепочке.
            var currentNext = next;

            // Фиксируем предыдущий next, чтобы новая лямбда вызвала следующий шаг, а не саму себя.
            next = () => behavior.HandleAsync(command, currentNext, cancellationToken);
        }

        return next();
    }
}
