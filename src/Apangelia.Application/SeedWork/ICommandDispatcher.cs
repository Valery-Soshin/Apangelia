namespace Apangelia.Application.SeedWork;

/// <summary>
/// Отправляет команды приложения в зарегистрированные обработчики.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Выполняет команду через обработчик и подключенные аспекты.
    /// </summary>
    /// <typeparam name="TResult">Тип результата выполнения команды.</typeparam>
    /// <param name="command">Команда приложения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения команды.</returns>
    Task<TResult> HandleAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken);
}
