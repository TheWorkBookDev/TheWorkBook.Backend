using Amazon.SimpleSystemsManagement;
using TheWorkBook.Utils.Abstraction;
using TheWorkBook.Utils.Abstraction.ParameterStore;

namespace TheWorkBook.Backend.API.Helper
{
    public class ParameterStoreHelper
    {
        readonly IEnvVariableHelper _envVariableHelper;

        public ParameterStoreHelper(IEnvVariableHelper envVariableHelper)
        {
            _envVariableHelper = envVariableHelper;
        }

        public IParameterStore GetParameterStore()
        {
            bool useSpecifiedParamStoreCreds =
                _envVariableHelper.GetVariable("UseSpecifiedParamStoreCreds") != null
                    && _envVariableHelper.GetVariable("UseSpecifiedParamStoreCreds")
                        .Equals("true", StringComparison.InvariantCultureIgnoreCase);

            AmazonSimpleSystemsManagementClient client;

            if (useSpecifiedParamStoreCreds)
            {
                Amazon.RegionEndpoint regionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_envVariableHelper.GetVariable("AWSRegion", true));

                client = new AmazonSimpleSystemsManagementClient(_envVariableHelper.GetVariable("ParamStoreConnectionKey", true),
                    _envVariableHelper.GetVariable("ParamStoreConnectionSecret", true), regionEndpoint);
            }
            else
            {
                client = new AmazonSimpleSystemsManagementClient();
            }

            return new TheWorkBook.Utils.ParameterStore.ParameterStore(client);
        }
    }
}
