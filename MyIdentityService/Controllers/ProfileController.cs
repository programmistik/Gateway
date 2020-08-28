using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private readonly APIService _apiService;

        public ProfileController(ProfileService profileService, IImageUploader imageUploader, UserManager<MyIdentityServiceUser> userManager, APIService apiService)
        {
            _profileService = profileService;
            _imageUploader = imageUploader;
            _userManager = userManager;
            _apiService = apiService;
        }

        private async Task<List<Post>> GetPostsAsync ()
        {
            
            var profile = _profileService.Get(User.Identity.Name);

            var UserId = profile.AppUserId;

            var response = await _apiService.callGetAPI("http://localhost:5012/posts/"+UserId);
            if (response.IsSuccessStatusCode)
            {
             
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Post>>(content);
            }
            return new List<Post>();
        }

        public async Task<IActionResult> UserProfile(int id = 1)
        {
            IEnumerable<Post> posts = await GetPostsAsync();
            var profile = _profileService.Get(User.Identity.Name);
            var friends = new List<Profile>();

            foreach (var item in profile.Friends)
            {
                var friendProfile = _profileService.Get(item);
                friends.Add(friendProfile);
            }

            return View(new ProfileAndPosts { Profile = profile, Posts = posts, Friends = friends, ActivePageNumber = id });
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
        {
            var profile = _profileService.Get(User.Identity.Name);
            prof.Id = profile.Id;
            prof.AppUserId = profile.AppUserId;
            prof.IdentityId = profile.IdentityId;
            prof.Friends = profile.Friends;

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
            var myProfile = _profileService.Get(User.Identity.Name);

            ViewBag.ShowButton = true;
            if (id == User.Identity.Name)
            {
                model.ActivePageNumber = 0;
                ViewBag.ShowButton = false;
            }

            else if (myProfile.Friends?.Where(x => x == id).Count() > 0)
                model.ActivePageNumber = 2;
            else
                model.ActivePageNumber = 1;

            return View(model);
        }

        private async Task<List<Post>> GetPostsAsyncById(string AppUserId)
        {

                var response = await _apiService.callGetAPI("http://localhost:5012/posts/" + AppUserId);
                if (response.IsSuccessStatusCode)
                {
                   
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Post>>(content);
                }
           
            return new List<Post>();
        }

        [HttpPost]
        public async Task jsAddFriend(string id)
        {
            var obj = new ReqFriendStr
            {
                Id = User.Identity.Name,
                //Method = "Add",
                UserId = id
            };

            var myProfile = _profileService.Get(User.Identity.Name);

            if (myProfile.Friends?.Where(x => x == id).Count() > 0)
                obj.Method = "Del"; 
            else
                obj.Method = "Add";

            await ChangeFriendListAsync(obj);
        }

        public async Task ChangeFriendListAsync(ReqFriendStr obj)
        {

            var js = JsonConvert.SerializeObject(obj);

            HttpContent cont = new StringContent(js, Encoding.UTF8, "application/json");

            var response = await _apiService.callPutAPI("http://localhost:5012/friends", cont);
            if (response.IsSuccessStatusCode)
            {
                
                var content = await response.Content.ReadAsStringAsync();
            }

        }

        public class ReqFriendStr
        {
            public string Id { get; set; }
            public string Method { get; set; }
            public string UserId { get; set; }
        }

    }
}