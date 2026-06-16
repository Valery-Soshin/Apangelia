using Apangelia.Application.SeedWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace Apangelia.Persistence.Postgres;

/// <summary>
/// Транзакционная единица работы для операций PostgreSQL-хранилища.
/// </summary>
public sealed class PostgresUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public PostgresUnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await operation(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await RollbackQuietlyAsync(transaction);
            throw;
        }
    }

    private static async Task RollbackQuietlyAsync(IDbContextTransaction transaction)
    {
        try
        {
            await transaction.RollbackAsync();
        }
        catch
        {
            // Ошибка отката не должна скрывать исходное исключение операции.
        }
    }
}
