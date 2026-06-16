namespace Apangelia.Application.Shared;

/// <summary>
/// Расширения для работы с исключениями.
/// </summary>
public static class ExceptionExtensions
{
    extension(Exception exception)
    {
        /// <summary>
        /// Проверяет, что исключение не является ожидаемой отменой переданного токена.
        /// </summary>
        public bool IsNotExpectedCancellation(CancellationToken cancellationToken)
        {
            return exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested;
        }
    }
}
