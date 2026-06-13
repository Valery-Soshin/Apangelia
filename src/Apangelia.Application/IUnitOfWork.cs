namespace Apangelia.Application;

/// <summary>
/// Выполняет операции приложения в общей транзакции.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Запускает действие внутри транзакции хранилища.
    /// </summary>
    /// <typeparam name="TResult">Тип результата транзакционной операции.</typeparam>
    /// <param name="operation">Операция, выполняемая внутри транзакции.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат операции.</returns>
    Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken);
}
