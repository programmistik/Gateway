using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using MyIdentityService.Helpers;
using MyIdentityService.Models;
using MyIdentityService.Services;
using MyIdentityService.ViewModels;
using Newtonsoft.Json;


namespace MyIdentityService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProfileService _profileService;
        private readonly APIService _apiService;

        public HomeController(ProfileService profileService, APIService apiService)
        {
            _profileService = profileService;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int spage = 1)
        {
            var page = spage;
            int pageSize = 3;   // количество элементов на странице


            var response = await _apiService.callGetAPI("http://localhost:5012/post/");


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dummyItems = JsonConvert.DeserializeObject<List<Post>>(content);
                var sortedItems = dummyItems.OrderByDescending(x => x.LikesProfileId.Count()).ToList();
                var count = sortedItems.Count();

                var items = sortedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                List<PostViewModel> Result = new List<PostViewModel>();

                if (items != null)
                    foreach (var item in items)
                    {
                        var prof = _profileService.Get(item.ProfileId);
                        var newItm = new PostViewModel
                        {
                            Post = item,
                            Profile = prof
                        };
                        Result.Add(newItm);

                    }
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    Posts = Result
                };
                return View(viewModel);
            }


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
            if (search?.Length > 0)
            {
                var res = await SearchAsync(search);
                return View(res);
            }
            else
            {
                return RedirectToAction("Index");
            }


        }

        private async Task<SearchingResult> SearchAsync(string str)
        {
            var Result = new SearchingResult();

           

            var response = await _apiService.callGetAPI("http://localhost:5012/profile/" + str);
          
            if (!response.IsSuccessStatusCode)
            {
                Result.Profiles = new List<Profile>();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Result.Profiles = JsonConvert.DeserializeObject<List<Profile>>(content);
            }

            var responsePost = await _apiService.callGetAPI("http://localhost:5012/search/" + str.ToLower());

            if (responsePost.IsSuccessStatusCode)
            {
               
                var contentPost = await responsePost.Content.ReadAsStringAsync();
                var rr = JsonConvert.DeserializeObject<List<Post>>(contentPost);
                Result.Posts = rr;
            }

            return Result;
        }
    }
}
