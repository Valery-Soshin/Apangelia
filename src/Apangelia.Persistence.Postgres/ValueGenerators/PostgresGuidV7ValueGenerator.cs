using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Apangelia.Persistence.Postgres.ValueGenerators;

internal sealed class PostgresGuidV7ValueGenerator : ValueGenerator<Guid>
{
    public override bool GeneratesTemporaryValues => false;

    public override Guid Next(EntityEntry entry)
    {
        return Create();
    }

    public static Guid Create()
    {
        return Guid.CreateVersion7();
    }
}
