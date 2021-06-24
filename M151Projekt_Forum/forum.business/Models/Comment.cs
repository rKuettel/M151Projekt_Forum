using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace forum.business.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int DiscussionId { get; set; }
        public Discussion Discussion { get; set; }
        public string Content { get; set; }
        public IdentityUser Commenter { get; set; }

        public Comment(int discussionId)
        {
            this.DiscussionId = discussionId;
        }
        public Comment() { }

    }
}