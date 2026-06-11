namespace Apangelia.Core;

public sealed class Notification
{
    public Guid Id { get; set; }

    public string Message { get; set; } = string.Empty;
}