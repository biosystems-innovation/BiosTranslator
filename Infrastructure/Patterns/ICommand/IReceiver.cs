namespace Infrastructure.Patterns.ICommand;

/// <summary>
/// https://refactoring.guru/es/design-patterns/command
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReceiver<T>
{
    T Result
    {
        get;
        set;
    }
}