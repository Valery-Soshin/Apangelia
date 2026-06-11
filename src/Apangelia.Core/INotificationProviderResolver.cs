namespace Apangelia.Core;

public interface INotificationProviderResolver
{
    INotificationProvider Resolve(string providerKey);
}