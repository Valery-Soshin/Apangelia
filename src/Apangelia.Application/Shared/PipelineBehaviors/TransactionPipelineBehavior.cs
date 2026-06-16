using Apangelia.Application.Abstractions.Persistence;
using Apangelia.Application.Shared.CommandBase;

namespace Apangelia.Application.Shared.PipelineBehaviors;

/// <summary>
/// Выполняет команду приложения внутри транзакционной единицы работы.
/// </summary>
/// <typeparam name="TCommand">Тип команды приложения.</typeparam>
/// <typeparam name="TResult">Тип результата выполнения команды.</typeparam>
public sealed class TransactionPipelineBehavior<TCommand, TResult>
    : IPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionPipelineBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<TResult> HandleAsync(
        TCommand command,
        CommandHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        if (command is INonTransactionalCommand)
        {
            return next();
        }

        return _unitOfWork.ExecuteInTransactionAsync(_ => next(), cancellationToken);
    }
}
