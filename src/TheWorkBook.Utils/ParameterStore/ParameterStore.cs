using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using TheWorkBook.Utils.Abstraction.ParameterStore;

namespace TheWorkBook.Utils.ParameterStore
{
    public sealed class ParameterStore : IParameterStore
    {
        private readonly string accessKey;
        private readonly Amazon.RegionEndpoint regionEndpoint;
        private readonly string secret;
        private IAmazonSimpleSystemsManagement _amazonSimpleSystemsManagementClient;
        private bool disposedValue;

        public ParameterStore(IAmazonSimpleSystemsManagement amazonSimpleSystemsManagementClient)
        {
            _amazonSimpleSystemsManagementClient = amazonSimpleSystemsManagementClient;
        }

        public ParameterStore(string accessKey, string secret)
        {
            this.accessKey = accessKey;
            this.secret = secret;
        }

        public ParameterStore(string accessKey, string secret, Amazon.RegionEndpoint regionEndpoint)
        {
            this.accessKey = accessKey;
            this.secret = secret;
            this.regionEndpoint = regionEndpoint;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IParameterList GetParameterListByPath(string path)
          => GetParameterListByPathAsync(path).GetAwaiter().GetResult();

        public async Task<IParameterList> GetParameterListByPathAsync(string path)
        {
            List<IParameter> parameters = await GetParametersByPathAsync(path);
            IParameterList parameterList = new ParameterList();

            foreach (IParameter parameter in parameters)
            {
                parameterList.AddParameter(parameter);
            }

            return parameterList;
        }

        public List<IParameter> GetParametersByPath(string path)
            => GetParametersByPathAsync(path).GetAwaiter().GetResult();

        public async Task<List<IParameter>> GetParametersByPathAsync(string path)
        {
            IAmazonSimpleSystemsManagement client = GetClient();

            GetParametersByPathRequest request = new GetParametersByPathRequest
            {
                Path = path,
                WithDecryption = true,
                Recursive = false
            };

            List<IParameter> parameters = await GetParametersByPathRecursiveAsync(client, request, 1);
            return parameters;
        }

        public void MakePutParameterRequest(IParameter parameter, bool withEncryption)
            => MakePutParameterRequestAsync(parameter, withEncryption).GetAwaiter().GetResult();

        public async Task MakePutParameterRequestAsync(IParameter parameter, bool withEncryption)
        {
            PutParameterRequest request = CreatePutParameterRequest(parameter, withEncryption);

            IAmazonSimpleSystemsManagement client = GetClient();
            PutParameterResponse response = await client.PutParameterAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new System.InvalidOperationException($"Critical Error: failed to put parameter: {parameter.Name} into Parameter Store");
        }

        public IParameter GetParameter(string fullPath)
        {
            return GetParameterAsync(fullPath).GetAwaiter().GetResult();
        }

        public async Task<IParameter> GetParameterAsync(string fullPath)
        {
            IAmazonSimpleSystemsManagement client = GetClient();

            GetParameterRequest request = new GetParameterRequest
            {
                Name = fullPath,
                WithDecryption = true
            };

            var response = await client.GetParameterAsync(request);
            return ToParameter(response.Parameter);
        }

        private static PutParameterRequest CreatePutParameterRequest(IParameter parameter,
            bool withEncryption)
        {
            var request = new PutParameterRequest
            {
                Description = parameter.Description,
                Name = parameter.Name,
                Overwrite = true,
                Type = withEncryption ? ParameterType.SecureString : ParameterType.String,
                Value = parameter.Value
            };

            return request;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _amazonSimpleSystemsManagementClient?.Dispose();
                }

                disposedValue = true;
            }
        }

        private IAmazonSimpleSystemsManagement GetClient()
        {
            // If using .NET Core you should already have this through DI.
            if (_amazonSimpleSystemsManagementClient != null)
                return _amazonSimpleSystemsManagementClient;

            if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secret) && regionEndpoint != null)
                _amazonSimpleSystemsManagementClient = new AmazonSimpleSystemsManagementClient(accessKey, secret, regionEndpoint);

            // App.config has: <add key="AWSRegion" value="eu-west-2" />
            if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secret))
                _amazonSimpleSystemsManagementClient = new AmazonSimpleSystemsManagementClient(accessKey, secret);

            if (_amazonSimpleSystemsManagementClient == null)
                _amazonSimpleSystemsManagementClient = new AmazonSimpleSystemsManagementClient();

            // <appSettings> <add key="AWSProfileName" value="AWS Default"/> </appSettings>
            return _amazonSimpleSystemsManagementClient;
        }

        private async Task<List<IParameter>> GetParametersByPathRecursiveAsync(IAmazonSimpleSystemsManagement client,
                                                    GetParametersByPathRequest request, int count)
        {
            List<IParameter> parameters = new List<IParameter>();
            GetParametersByPathResponse response = await client.GetParametersByPathAsync(request);

            response.Parameters.ForEach(par =>
            {
                parameters.Add(ToParameter(par));
            });

            if (!string.IsNullOrWhiteSpace(response.NextToken) && count < 3)
            {
                // Go again
                count++;
                request.NextToken = response.NextToken;
                List<IParameter> params2 = await GetParametersByPathRecursiveAsync(client, request, count);
                parameters.AddRange(params2);
            }

            return parameters;
        }

        private Parameter ToParameter(Amazon.SimpleSystemsManagement.Model.Parameter parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            return new Parameter
            {
                Name = parameter.Name,
                Value = parameter.Value,
                DataType = parameter.DataType,
                LastModifiedDate = parameter.LastModifiedDate
            };
        }
    }
}
