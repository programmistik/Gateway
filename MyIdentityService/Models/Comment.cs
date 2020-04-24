using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Models
{
    public class Comment
    {
        public string PostId { get; set; }
        public string CommentId { get; set; }
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string ProfileName { get; set; }
        public string ProfileAvatara { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public List<Comment> Comments { get; set; }
    }
   
}
