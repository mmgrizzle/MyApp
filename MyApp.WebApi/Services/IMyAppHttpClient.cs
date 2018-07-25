using System.Net.Http;
using System.Threading.Tasks;

namespace MyApp.WebApi.Services
{
    public interface IMyAppHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
