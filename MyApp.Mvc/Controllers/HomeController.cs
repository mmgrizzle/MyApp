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

        //method launches the about view- requires admin role and api access to view suppliers & user address
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            //gets user info about authorization from the UserInfoEndpoint
            var discoveryClient = new DiscoveryClient("https://localhost:44357/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            //response from user endpoint
            var userResponse = await userInfoClient.GetAsync(accessToken);

            if (userResponse.IsError)  //error handling for user info endpoint access
            {
                throw new Exception(
                    "Problem accessing the UserInfo endpoint."
                    , userResponse.Exception);
            }

            // call the API
            var httpClient = await _myAppHttpClient.GetClient();

            //response from http api call
            var httpResponse = await httpClient.GetAsync("api/suppliers").ConfigureAwait(false);


            if (httpResponse.IsSuccessStatusCode)  //validates httpResponse 
            {
                //creates a value from the claims in the user info endpoint
                var address = userResponse.Claims.FirstOrDefault(c => c.Type == "address")?.Value;

                //puts the suppliers from the api into a string
                var suppliersAsString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                //creates a view model from the address and the suppliers 
                var aboutViewModel = new AboutViewModel(address,
                    JsonConvert.DeserializeObject<IList<Supplier>>(suppliersAsString).ToList());

                return View(aboutViewModel);
            }

            //returns access denied if unauthorized user 
            else if(httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                    httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }
            throw new Exception ($"A problem happened while calling the API: {httpResponse.ReasonPhrase}");
           
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
