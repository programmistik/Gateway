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
using MyIdentityService.ViewModels;
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



            var response = await client.PostAsync("http://localhost:5012/post", cont);
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
            newPost.Comments = new List<Comment>();
            newPost.Image = await _imageUploader.Upload(Image);

            await AddNewPostAsync(newPost);

            return RedirectToAction("UserProfile", "Profile", new { id = 2 });
        }

        public void fillProfiles(List<Comment> comments)
        {
            foreach (var item in comments)
            {
                item.Profile = _profileService.Get(item.ProfileId);
                if (item.Comments.Count() > 0)
                    fillProfiles(item.Comments);
            }

        }

        public async Task<IActionResult> Post(string id)
        {
            var vm = new PostViewModel();
            vm.VVM = new VueViewModel();
            var parser = new VueParser(); // in the real app you would use DI
            vm.VVM.VueData = parser.ParseData(vm.VVM);

            var currPost = await GetPostByIdAsync(id);
            if(currPost.Comments != null)
                fillProfiles(currPost.Comments);

            vm.Profile = _profileService.Get(currPost.Profile.AppUserId);
            vm.Post = currPost;

            if (currPost.Profile.AppUserId == User.Identity.Name)
            {
                vm.Owener = true;
                vm.CurrUserProfile = JsonConvert.SerializeObject(vm.Profile);
            }
            else
            {
                vm.Owener = false;
                vm.CurrUserProfile = JsonConvert.SerializeObject(_profileService.Get(User.Identity.Name));

                // add view if needed
                var viewed = currPost.ViewsProfileId.Where(x => x.Equals(User.Identity.Name)).FirstOrDefault();
                if (viewed == null)
                {
                    currPost.ViewsProfileId.Add(User.Identity.Name);

                    //CONNECT
                    var client = new HttpClient();
                    var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
                    if (disco.IsError)
                    {
                        // Console.WriteLine(disco.Error);
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
                        // Console.WriteLine(tokenResponse.Error);
                        //return;
                    }
                    //Console.WriteLine(tokenResponse.Json);

                    //CALL API
                    //var obj = new ReqStrViews
                    //{
                    //    Id = id,
                    //    UserId = User.Identity.Name
                    //};


                    var js = JsonConvert.SerializeObject(new 
                    {
                        Id = id,
                        UserId = User.Identity.Name
                    });


                    client.SetBearerToken(tokenResponse.AccessToken);
                    HttpContent cont = new StringContent(js, Encoding.UTF8, "application/json");



                    var response = await client.PutAsync("http://localhost:5012/views", cont);
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


            if (currPost.LikesProfileId.Contains(User.Identity.Name))
                ViewBag.Liked = true;
            else
                ViewBag.Liked = false;



            return View(vm);
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
            // Console.WriteLine(tokenResponse.Json);

            //CALL API
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5012/post/" + id);
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



            var response = await client.PutAsync("http://localhost:5012/posts", cont);
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

        [HttpDelete]
        public async Task jsDelPost(string id)
        {

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


            var response = await client.DeleteAsync("http://localhost:5012/posts/" + id);
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine(response.StatusCode);
                // return RedirectToAction("UserProfile", "Profile", new { id = 2 });
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(content);
            }


        }

        public async Task<IActionResult> News()
        {
            List<PostViewModel> Result = new List<PostViewModel>();

            var myProfile = _profileService.Get(User.Identity.Name);
            var myId = User.Identity.Name;

            //foreach (var itm in myProfile.Friends)
            //{
            //    var id = itm;


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
                
                var posts = new List<Post>();

            //var response = await client.GetAsync("http://localhost:5000/views/" + id + "/"+myId);
            var response = await client.GetAsync("http://localhost:5012/views/" + "smthg" + "/" + myId);
            if (!response.IsSuccessStatusCode)
                {
                    //  Console.WriteLine(response.StatusCode);
                    // Result = new List<PostViewModel>();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<Post>>(content);

                    if (posts != null)
                        foreach (var item in posts)
                        {
                            if (item.ProfileId != myId)
                            {
                                var prof = _profileService.Get(item.ProfileId);
                                var newItm = new PostViewModel
                                {
                                    Post = item,
                                    Profile = prof
                                };
                                Result.Add(newItm);
                            }

                        }
               // }
                
            }
            return View(new NewsViewModel() {NewsList = Result, news = JsonConvert.SerializeObject(Result) } );
        }

        public async Task<IActionResult> EditPost(string id)
        {
            var currPost = await GetPostByIdAsync(id);

            return View(currPost);
        }

        public async Task<IActionResult> ChangePost(Post post, string id)
        {
            var currPost = await GetPostByIdAsync(id);
            post.Id = currPost.Id;
            post.ProfileId = currPost.ProfileId;
            post.Profile = _profileService.Get(currPost.ProfileId);
            post.Date = currPost.Date;
            post.Image = currPost.Image;
            post.LikesProfileId = currPost.LikesProfileId;
            post.ViewsProfileId = currPost.ViewsProfileId;
            post.Comments = currPost.Comments;

            //CONNECT
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                // Console.WriteLine(disco.Error);
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
                // Console.WriteLine(tokenResponse.Error);
                //return;
            }
            //Console.WriteLine(tokenResponse.Json);

            //CALL API



            var js = JsonConvert.SerializeObject(post);


            client.SetBearerToken(tokenResponse.AccessToken);
            HttpContent cont = new StringContent(js, Encoding.UTF8, "application/json");



            var response = await client.PutAsync("http://localhost:5012/post", cont);
            if (!response.IsSuccessStatusCode)
            {
                //Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(content);
            }



            return RedirectToAction("UserProfile", "Profile", new { id = 2 });
        }

        [HttpPost]
        public async Task jsAddComment(string id, string CommentId, string Obj, string Text)
        {
            // create new comment object

            var prof = JsonConvert.DeserializeObject<Profile>(Obj);

            var newComment = new Comment
            {
                CommentDate = DateTime.Now,
                CommentId = CommentId,
                Comments = new List<Comment>(),
                CommentText = Text,
                ProfileId = prof.Id,
                Profile = prof
            };

            var post = //await GetPostByIdAsync(id);
                new PostComment
                {
                    Id = id,
                    Comment = newComment
                };

           // post.Comments.Add(newComment);


            //CONNECT
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                // Console.WriteLine(disco.Error);
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
                // Console.WriteLine(tokenResponse.Error);
                //return;
            }
            //Console.WriteLine(tokenResponse.Json);

            //CALL API



            var js = JsonConvert.SerializeObject(post);


            client.SetBearerToken(tokenResponse.AccessToken);
            HttpContent cont = new StringContent(js, Encoding.UTF8, "application/json");



            var response = await client.PutAsync("http://localhost:5012/comments", cont);
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

    public class ReqStrViews
    {
        public string Id { get; set; } // postId
        
        public string UserId { get; set; }
    }
    public class PostComment
    {
        public string Id { get; set; } // postId

        public Comment Comment { get; set; }
    }
}