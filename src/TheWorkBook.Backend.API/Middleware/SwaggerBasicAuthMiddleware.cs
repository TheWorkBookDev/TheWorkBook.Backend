using System.Net;
using System.Text;
using TheWorkBook.Backend.API.Helper;
using TheWorkBook.Utils.Abstraction;
using TheWorkBook.Utils.Abstraction.ParameterStore;

namespace TheWorkBook.Backend.API.Middleware
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate next;
        readonly IEnvVariableHelper _envVariableHelper;

        public SwaggerBasicAuthMiddleware(RequestDelegate next, IEnvVariableHelper envVariableHelper)
        {
            this.next = next;
            _envVariableHelper = envVariableHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
            if (context.Request.Path.StartsWithSegments("/swagger") && !this.IsLocalRequest(context))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    string? encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                    
                    if (string.IsNullOrWhiteSpace(encodedUsernamePassword))
                    {
                        // Bad Request - Request does not contain Basic Auth credentials.
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        await next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context);
            }
        }

        public bool IsAuthorized(string username, string password)
        {
            ParameterStoreHelper parameterStoreHelper = new ParameterStoreHelper(_envVariableHelper);
            using IParameterStore parameterStore = parameterStoreHelper.GetParameterStore();

            IParameterList parameterList = parameterStore.GetParameterListByPath("/swagger/auth/");

            string user = "admin";
            string secret = "password";

            if (parameterList != null)
            {
                user = parameterList.GetParameterValue("Username");
                if (string.IsNullOrWhiteSpace(user))
                    user = "admin";

                secret = parameterList.GetParameterValue("Secret");
                if (string.IsNullOrWhiteSpace(secret))
                    secret = "password";
            }

            // Check that username and password are correct
            return username.Equals(user, StringComparison.InvariantCultureIgnoreCase)
                    && password.Equals(secret);
        }

        public bool IsLocalRequest(HttpContext context)
        {
            //Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
            if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
            {
                return true;
            }

            if (context.Connection.RemoteIpAddress != null)
            {
                if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
                {
                    return true;
                }
                
                if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
