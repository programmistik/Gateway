using MyIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.ViewModels
{
    public class NewsViewModel
    {
        public List<PostViewModel> NewsList { get; set; }
        public string news { get; set; }
    }
}
