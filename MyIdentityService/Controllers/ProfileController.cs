using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdentityService.Areas.Identity.Data;
using MyIdentityService.Models;
using MyIdentityService.Services;
using MyIdentityService.ViewModels;
using Newtonsoft.Json;

namespace MyIdentityService.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileService _profileService;
        private readonly IImageUploader _imageUploader;
        private readonly UserManager<MyIdentityServiceUser> _userManager;

        public ProfileController(ProfileService profileService, IImageUploader imageUploader, UserManager<MyIdentityServiceUser> userManager)
        {
            _profileService = profileService;
            _imageUploader = imageUploader;
            _userManager = userManager;
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

            //CALL API
            client.SetBearerToken(tokenResponse.AccessToken);

            var profile = _profileService.Get(User.Identity.Name);

            var UserId = profile.AppUserId;

            var response = await client.GetAsync("http://localhost:5000/posts/"+UserId);
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

        //private async Task<List<Post>> GetAllPosts()
        //{
        //    return  await GetPostsAsync();           
        //}

        public async Task<IActionResult> UserProfile(int id = 1)
        {
            IEnumerable<Post> posts = await GetPostsAsync();
            var profile = _profileService.Get(User.Identity.Name);

            return View(new ProfileAndPosts { Profile = profile, Posts = posts, ActivePageNumber = id });
        }

        public IActionResult Settings(string id)
        {
            return View();
        }

        public IActionResult ChangeProfile()
        {
            

            return View(_profileService.Get(User.Identity.Name));
        }

        public async Task<IActionResult> ChangeProfileInfo(Profile prof, string Avatara, string Ava)
        //     public async Task<IActionResult> ChangeProfileInfo(Profile prof, IFormFile Avatara)
        {
            var profile = _profileService.Get(User.Identity.Name);
            prof.Id = profile.Id;
            prof.AppUserId = profile.AppUserId;
            prof.IdentityId = profile.IdentityId;
            if (Ava == null)
            {
                prof.Avatara = profile.Avatara;
            }
            else
            {
                prof.Avatara = await _imageUploader.UploadAva(Ava, profile.IdentityId);
            }
            _profileService.Update(profile.Id, prof);

            return RedirectToAction("UserProfile");
        }

        public async Task<IActionResult> ProfileView(string id)
        {
            IEnumerable<Post> posts = await GetPostsAsyncById(id);
            var profile = _profileService.Get(id);
            

            var model = new ProfileAndPosts { Profile = profile, Posts = posts };

            if (id == User.Identity.Name)
                model.ActivePageNumber = 0;

            else if (profile.Friends?.Where(x => x == id).Count() > 0)
                model.ActivePageNumber = 2;
            else
                model.ActivePageNumber = 1;

            return View(model);
        }

        private async Task<List<Post>> GetPostsAsyncById(string AppUserId)
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

            if (!tokenResponse.IsError)
            {
                //CALL API
                client.SetBearerToken(tokenResponse.AccessToken);


                var response = await client.GetAsync("http://localhost:5000/posts/" + AppUserId);
                if (!response.IsSuccessStatusCode)
                {
                    //  Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Post>>(content);
                }
            }

            return new List<Post>();
        }
    }
}