using MyIdentityService.Models;
using MyIdentityService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Helpers
{
    public class IndexViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
