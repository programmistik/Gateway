using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using MyIdentityService.Models;
using MyIdentityService.Services;
using MyIdentityService.ViewModels;
using Newtonsoft.Json;

namespace MyIdentityService.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        private async Task<List<Post>> GetPostsAsync ()
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
           
            var response = await client.GetAsync("http://localhost:5000/post"); 
            if (!response.IsSuccessStatusCode)
            {
              //  Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Post>>(content);
            }
            return new List<Post>();
        }

        private async Task<List<Post>> GetAllPosts()
        {
            return  await GetPostsAsync();           
        }

        public async Task<IActionResult> UserProfile()
        {
            IEnumerable<Post> posts = await GetAllPosts(); //await GetPostsAsync();
            var profile = _profileService.Get(User.Identity.Name);

            return View(new ProfileAndPosts { Profile = profile, Posts = posts });
        }

        public IActionResult Settings(string id)
        {
            return View();
        }

        public IActionResult ChangeProfile()
        {
            

            return View();
        }
        public IActionResult ChangeProfileInfo(Profile prof)
        {
            var profile = _profileService.Get(User.Identity.Name);
            prof.Id = profile.Id;
            prof.AppUserId = profile.AppUserId;
            _profileService.Update(profile.Id, prof);

            return RedirectToAction("UserProfile");
        }
    }
}