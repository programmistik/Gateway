using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyIdentityService.Models;
using MyIdentityService.Services;
using Newtonsoft.Json;

namespace MyIdentityService
{

    //[Route("api/[controller]")]
    //[ApiController]
    public class PostController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public PostController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task AddNewPost()
        {
            var profile = _profileService.Get(User.Identity.Name);
            var newPost = new Post
            {
                ProfileId = profile.AppUserId,
                Title = "Image",
                Date = DateTime.Now,
                Description = "Something about this photo",
                Image = "s-l500.jpg",
                Profile = profile,
                LikesProfileId = new List<string>(),
                ViewsProfileId = new List<string>()
            };

            

            var js = JsonConvert.SerializeObject(newPost);

            //CONNECT
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                // Console.WriteLine(disco.Error);
                return;
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
                // Console.WriteLine(tokenResponse.Error);
                return;
            }
            //Console.WriteLine(tokenResponse.Json);

            //CALL API
            client.SetBearerToken(tokenResponse.AccessToken);
            HttpContent cont = new StringContent(js, Encoding.UTF8, "application/json");

            

            var response = await client.PostAsync("http://localhost:5000/post", cont); 
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(content);
            }

        }
    }
}