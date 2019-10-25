using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public interface IHttpClient
    {
        // uri => Path of Microservice
        // Get
        Task<string> GetStringAsync(string uri);
        // Post
        Task<HttpResponseMessage> PostAsync<T>(string uri, T item);
        // Delete
        Task<HttpResponseMessage> DeleteAsync(string uri);
        // Put 
        Task<HttpResponseMessage> PutAsync<T>(string uri, T item);
    }
}
