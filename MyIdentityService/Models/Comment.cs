using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Models
{
    public class Comment
    {
        public string CommentId { get; set; }
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
