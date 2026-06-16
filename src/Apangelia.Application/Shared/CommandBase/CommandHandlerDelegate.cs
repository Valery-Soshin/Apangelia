namespace Apangelia.Application.Shared.CommandBase;

/// <summary>
/// Представляет следующий шаг выполнения команды в цепочке обработчиков.
/// </summary>
/// <typeparam name="TResult">Тип результата выполнения команды.</typeparam>
/// <returns>Результат выполнения следующего шага.</returns>
public delegate Task<TResult> CommandHandlerDelegate<TResult>();
