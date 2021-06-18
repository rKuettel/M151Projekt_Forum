using forum.business.DataAccess;
using forum.business.Data;
using forum.business.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace forum.business.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly ForumDbContext context;

        public DataAccess(ForumDbContext context)
        {
            this.context = context;
        }

        public List<Discussion> GetAllDiscussions()
        {
            var discussions = context.Discussions.Select(d => new Discussion
            {
                Id = d.Id,
                Title = d.Title,
                Author = new IdentityUser
                {
                    // ToDo: Add test that username is not null
                    Id = d.Author.Id,
                    UserName = d.Author.UserName
                },
                comments = d.comments,
                Content = d.Content,
                Pictures = d.Pictures
            });

            var discussionsList = discussions.ToList();
            return discussions.ToList();
        }

        public Discussion GetDiscussionById(int discussionId)
        {
            var discussion = GetAllDiscussions().FirstOrDefault(d => d.Id == discussionId);
            return discussion;
        }

        public void CreateNewDiscussion(Discussion discussion)
        {
            discussion.Id = 0;
            var author = context.Users.FirstOrDefault(u => u.Id == discussion.Author.Id);
            discussion.Author = author;
            context.Discussions.Add(discussion);
            context.SaveChanges();
        }
    }
}
