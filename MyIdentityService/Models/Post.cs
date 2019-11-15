using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public List<int> ViewsProfileId { get; set; }
        public List<int> LikesProfileId { get; set; }
    }
}
