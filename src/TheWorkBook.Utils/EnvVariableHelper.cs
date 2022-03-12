using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWorkBook.Utils.Abstraction;

namespace TheWorkBook.Utils
{
    public class EnvVariableHelper : IEnvVariableHelper
    {
        protected ILogger<EnvVariableHelper> _logger;
        private readonly IConfiguration _configuration;

        public EnvVariableHelper(ILogger<EnvVariableHelper> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string GetVariable(string variableName)
        {
            var value = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrWhiteSpace(value) && _configuration != null) {
                value = _configuration[$"AppSettings:{variableName}"];
            }

            if (_logger != null) {
                if (!string.IsNullOrWhiteSpace(value)) {
                    _logger.LogTrace("Retrieved Environment Variable: {variableName}: {value}", variableName, value);
                }
                else {
                    _logger.LogTrace($"Environment Variable: {variableName} not found");
                }
            }

            if (string.IsNullOrWhiteSpace(value)) {
                return String.Empty;
            }

            return value;
        }

        public string GetVariable(string variableName, bool throwIfNull)
        {
            string value = GetVariable(variableName);
            if (string.IsNullOrWhiteSpace(value) && throwIfNull) {
                throw new ArgumentException($"Environment Variable: '{variableName}' not found.");
            }

            return value;
        }

        public T? GetVariable<T>(string variableName)
        {
            string value = GetVariable(variableName);
            return Change.To<T>(value);
        }

        public T? GetVariable<T>(string variableName, bool throwIfNull)
        {
            string value = GetVariable(variableName, throwIfNull);
            return Change.To<T>(value);
        }
    }
}
