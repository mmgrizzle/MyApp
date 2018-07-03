using System.Net.Http;
using System.Threading.Tasks;

namespace MyApp.Mvc.Services
{
    public interface IMyAppHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
