using MyIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.ViewModels
{
    public class PostViewModel
    {
        public Profile Profile { get; set; }
        public Post Post { get; set; }
        public bool Owener { get; set; }
        public string CurrUserProfile { get; set; }
        public VueViewModel VVM { get; set; }
    }
}
