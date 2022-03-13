namespace TheWorkBook.Utils.Abstraction.ParameterStore
{
    public interface IParameterList
    {
        void AddParameter(IParameter parameter);
        IParameter GetParameter(string key);
        string GetParameterValue(string key);
    }
}
