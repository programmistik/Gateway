using MyIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.ViewModels
{
    public class ProfileAndPosts
    {
        public Profile Profile { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Profile> Friends { get; set; }
        public int ActivePageNumber { get; set; }
    }
}
