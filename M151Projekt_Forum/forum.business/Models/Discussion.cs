using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace forum.business.Models
{
    public class Discussion
    {
        public int Id { get; set; }
        public IdentityUser Author { get; set; }
        public string Title { get; set; }
        public virtual List<Picture> Pictures { get; set; }
        public string Content { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
