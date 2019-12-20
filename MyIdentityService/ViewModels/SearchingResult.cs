using MyIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.ViewModels
{
    public class SearchingResult
    {
        public IEnumerable<Profile> Profiles { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
