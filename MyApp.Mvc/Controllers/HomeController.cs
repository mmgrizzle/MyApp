using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MyApp.Api.Models;
using MyApp.Mvc.Models;
using MyApp.Mvc.Services;
using MyApp.Mvc.ViewModels;
using Newtonsoft.Json;

namespace MyApp.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMyAppHttpClient _myAppHttpClient;

        public HomeController(IMyAppHttpClient myAppHttpClient)
        {
            _myAppHttpClient = myAppHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            await WriteOutIdentityInformation();
            return View();
        }

        //public async Task<IActionResult> API()
        //{
        //    var httpClient = await _myAppHttpClient.GetClient();
        //    var response = await httpClient.GetAsync("api/suppliers").ConfigureAwait(false);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var suppliersAsString = await response.Content.ReadAsStringAsync().ConfigureAwait
        //    }
            
        //}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

           
            // call the API
           
            var discoveryClient = new DiscoveryClient("https://localhost:44357/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfoClient.GetAsync(accessToken);

            var httpClient = await _myAppHttpClient.GetClient();
            var httpResponse = await httpClient.GetAsync("api/suppliers").ConfigureAwait(false);


            if (httpResponse.IsSuccessStatusCode)
            {
                var address = response.Claims.FirstOrDefault(c => c.Type == "address")?.Value;

                var suppliersAsString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                var aboutViewModel = new AboutViewModel(address,
                    JsonConvert.DeserializeObject<IList<Supplier>>(suppliersAsString).ToList());

                return View(aboutViewModel);
            }

            if (response.IsError)
            {
                throw new Exception(
                    "Problem accessing the UserInfo endpoint."
                    , response.Exception);
            }
            else
                return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public async Task WriteOutIdentityInformation()
        {
            var identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token {identityToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}
