namespace TheWorkBook.Utils.Abstraction
{
    public interface IEnvVariableHelper
    {
        string GetVariable(string variableName);
        string GetVariable(string variableName, bool throwIfNull);

        T? GetVariable<T>(string variableName);
        T? GetVariable<T>(string variableName, bool throwIfNull);
    }
}