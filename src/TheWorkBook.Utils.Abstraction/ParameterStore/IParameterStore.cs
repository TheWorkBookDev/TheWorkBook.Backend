namespace TheWorkBook.Utils.Abstraction.ParameterStore
{
    public interface IParameterStore : IDisposable
    {
        IParameter GetParameter(string fullPath);
        Task<IParameter> GetParameterAsync(string fullPath);
        IParameterList GetParameterListByPath(string path);
        Task<IParameterList> GetParameterListByPathAsync(string path);
        List<IParameter> GetParametersByPath(string path);
        Task<List<IParameter>> GetParametersByPathAsync(string path);
        void MakePutParameterRequest(IParameter parameter, bool withEncryption);
        Task MakePutParameterRequestAsync(IParameter parameter, bool withEncryption);
    }
}
