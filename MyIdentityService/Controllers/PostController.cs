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
    public class PostController : Controller
    {
        private readonly ProfileService _profileService;
        private readonly IImageUploader _imageUploader;

        public PostController(ProfileService profileService, IImageUploader imageUploader)
        {
            _profileService = profileService;
            _imageUploader = imageUploader;
        }

        public async Task AddNewPostAsync(Post newPost)
        {


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

        public IActionResult AddNewPost()
        {


            return View(new Post());
        }

        public async Task<IActionResult> NewPost(Post newPost, IFormFile Image)
        {
            var profile = _profileService.Get(User.Identity.Name);

            newPost.Id = Guid.NewGuid().ToString();
            newPost.ProfileId = profile.AppUserId;
            newPost.Date = DateTime.Now;
            newPost.Profile = profile;
            newPost.LikesProfileId = new List<string>();
            newPost.ViewsProfileId = new List<string>();
            newPost.Image = await _imageUploader.Upload(Image);

            await AddNewPostAsync(newPost);

            return RedirectToAction("UserProfile", "Profile", new { id = 2 });
        }

        public async Task<IActionResult> Post(string id)
        {
            var currPost = await GetPostByIdAsync(id);
            if (currPost.LikesProfileId.Contains(User.Identity.Name))
                ViewBag.Color = true;
            else
                ViewBag.Color = false;

            return View(currPost);
        }

        private async Task<Post> GetPostByIdAsync(string id)
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

            //var client2 = new HttpClient();
            //var response = await client.GetAsync("http://localhost:3300/" + id);
            var response = await client.GetAsync("http://localhost:5000/post/"+id);
            if (!response.IsSuccessStatusCode)
            {
                //  Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Post>(content);
            }
            return new Post();
        }

        [HttpPost]
        public async Task jsAddLike(string id)
        {
            var currPost = await GetPostByIdAsync(id);

            var ImgCreatorId = currPost.ProfileId;

            var currUserId = User.Identity.Name;

            //if (ImgCreatorId != currUserId)
            //// You cant add like to your own image

            //{
            var obj = new ReqStr
            {
                Id = currPost.Id,
                ArrayName = "Likes",
                //Method = "Add",
                UserId = User.Identity.Name
            };

            if (currPost.LikesProfileId.Where(x => x == User.Identity.Name).Count() == 0)
            // You cant add like if you already like 
            {
                obj.Method = "Add";
            }
            else
            {
                obj.Method = "Del";
                //currPost.LikesProfileId.Add(currUserId);

            }

            await ChangePostAsync(obj);
            //    }
            //}
        }

        public async Task ChangePostAsync(ReqStr obj)
        {

           // var js = JsonConvert.SerializeObject(editPost);

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

           

            var js = JsonConvert.SerializeObject(obj);


            client.SetBearerToken(tokenResponse.AccessToken);
            HttpContent cont = new StringContent(js, Encoding.UTF8, "application/json");



            var response = await client.PutAsync("http://localhost:5000/posts", cont);
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

    public class ReqStr
    {
        public string Id { get; set; }
        public string ArrayName { get; set; }
        public string Method { get; set; }
        public string UserId { get; set; }
    }
}