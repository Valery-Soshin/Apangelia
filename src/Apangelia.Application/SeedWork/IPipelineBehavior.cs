namespace Apangelia.Application.SeedWork;

/// <summary>
/// Выполняет аспект вокруг обработчика команды приложения.
/// </summary>
/// <typeparam name="TCommand">Тип команды приложения.</typeparam>
/// <typeparam name="TResult">Тип результата выполнения команды.</typeparam>
public interface IPipelineBehavior<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Выполняет аспект и передает управление следующему шагу цепочки.
    /// </summary>
    /// <param name="command">Команда приложения.</param>
    /// <param name="next">Следующий шаг выполнения команды.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения команды.</returns>
    Task<TResult> HandleAsync(
        TCommand command,
        CommandHandlerDelegate<TResult> next,
        CancellationToken cancellationToken);
}
