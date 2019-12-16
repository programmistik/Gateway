using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using MyIdentityService.Models;
using Newtonsoft.Json;

namespace MyIdentityService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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

        public async Task<IActionResult> Search(string search)
        {
            if (search.Length > 0)
            {
                var list = await SearchAsync(search);
                return View(list);
            }
            else
            {
                return RedirectToAction("Index");
            }

            
        }

        private async Task<List<Profile>> SearchAsync(string str)
        {
            //CONNECT
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                //Console.WriteLine(disco.Error);
                //return;
            }

            //GET TOKEN
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "Post"
            });
            if (tokenResponse.IsError)
            {
                //Console.WriteLine(tokenResponse.Error);
                //return;
            }
            Console.WriteLine(tokenResponse.Json);

            //CALL API
            client.SetBearerToken(tokenResponse.AccessToken);

            
            var response = await client.GetAsync("http://localhost:5000/search/" + str);
            if (!response.IsSuccessStatusCode)
            {
                //  Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Profile>>(content);
            }
            return new List<Profile>();
        }
    }
}
