namespace Apangelia.Application.SeedWork;

/// <summary>
/// Обрабатывает команду приложения заданного типа.
/// </summary>
/// <typeparam name="TCommand">Тип команды приложения.</typeparam>
/// <typeparam name="TResult">Тип результата выполнения команды.</typeparam>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Выполняет команду приложения.
    /// </summary>
    /// <param name="command">Команда приложения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения команды.</returns>
    Task<TResult> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}
