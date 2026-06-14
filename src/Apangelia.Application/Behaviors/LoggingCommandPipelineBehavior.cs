using System.Diagnostics;
using Apangelia.Application.Commands;
using Microsoft.Extensions.Logging;

namespace Apangelia.Application.Behaviors;

/// <summary>
/// Логирует выполнение команды приложения без записи полезной нагрузки команды и результата.
/// </summary>
/// <typeparam name="TCommand">Тип команды приложения.</typeparam>
/// <typeparam name="TResult">Тип результата выполнения команды.</typeparam>
public sealed class LoggingCommandPipelineBehavior<TCommand, TResult>
    : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ILogger<LoggingCommandPipelineBehavior<TCommand, TResult>> _logger;

    public LoggingCommandPipelineBehavior(ILogger<LoggingCommandPipelineBehavior<TCommand, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(
        TCommand command,
        CommandHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        var commandType = typeof(TCommand).Name;
        var resultType = typeof(TResult).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Command '{CommandType}' started with result type {ResultType}.",
            commandType,
            resultType);

        try
        {
            var result = await next();
            stopwatch.Stop();

            _logger.LogInformation(
                "Command '{CommandType}' completed in {ElapsedMilliseconds} ms.",
                commandType,
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            _logger.LogError(
                exception,
                "Command '{CommandType}' failed in {ElapsedMilliseconds} ms.",
                commandType,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
