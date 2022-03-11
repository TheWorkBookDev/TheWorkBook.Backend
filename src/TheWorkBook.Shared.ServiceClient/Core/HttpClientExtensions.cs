using System.Text;
using System.Text.Json;

namespace TheWorkBook.Shared.ServiceClient.Core
{
    public static class HttpClientExtensions
    {
        public static async Task<T> MakeGetRequest<T>(this HttpClient httpClient, string requestString)
        {
            Uri requestUri = new Uri(requestString);

            return await MakeGetRequest<T>(httpClient, requestUri);
        }

        public static async Task<T> MakeGetRequest<T>(this HttpClient httpClient, Uri requestUri)
        {
            using (var httpResponse = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
                return await httpResponse.Content.ReadAsAsync<T>();
            }
        }

        public static async Task<T> MakePostRequest<T, U>(this HttpClient httpClient, Uri requestUri, U postData)
        {
            using (var httpResponse = await httpClient.PostAsJsonAsync(requestUri, postData))
            {
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
                return await httpResponse.Content.ReadAsAsync<T>();
            }
        }

        public static async Task<T> MakePutRequest<T, U>(this HttpClient httpClient, Uri requestUri, U postData)
        {
            using (var httpResponse = await httpClient.PutAsJsonAsync(requestUri, postData))
            {
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
                return await httpResponse.Content.ReadAsAsync<T>();
            }
        }

        public static async Task<T> MakeDeleteRequest<T, U>(this HttpClient httpClient, Uri requestUri, U postData)
        {
            var json = JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Content = content;

            using (var httpResponse = await httpClient.SendAsync(request))
            {
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
                return await httpResponse.Content.ReadAsAsync<T>();
            }
        }

        public static async Task<T> MakePatchRequest<T, U>(this HttpClient httpClient, Uri requestUri, U postData)
        {
            var serializedDoc = Newtonsoft.Json.JsonConvert.SerializeObject(postData);
            var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");

            using (var httpResponse = await httpClient.PatchAsync(requestUri, requestContent))
            {
                httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
                return await httpResponse.Content.ReadAsAsync<T>();
            }
        }
    }
}
