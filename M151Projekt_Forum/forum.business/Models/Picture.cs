using System;
using Microsoft.AspNetCore.Identity;

namespace forum.business.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int DiscussionId { get; set; }
    }
}