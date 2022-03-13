using TheWorkBook.Utils.Abstraction.ParameterStore;

namespace TheWorkBook.Utils.ParameterStore
{
    public sealed class ParameterList : IParameterList
    {
        private readonly List<IParameter> Parameters;

        public ParameterList()
        {
            Parameters = new List<IParameter>();
        }

        public void AddParameter(IParameter parameter)
        {
            Parameters.Add(parameter);
        }

        public IParameter GetParameter(string key)
        {
            return Parameters.FirstOrDefault(p => p.Name == key || p.FriendlyName == key);
        }

        public string GetParameterValue(string key)
        {
            IParameter parameter = GetParameter(key);
            return parameter.Value;
        }
    }
}
