using System;
using Microsoft.AspNetCore.Identity;

namespace forum.business.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        //public IdentityUser UploadedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DiscussionId { get; set; }
    }
}