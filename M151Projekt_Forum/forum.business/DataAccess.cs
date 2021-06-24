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
                Comments = d.Comments,
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

        public void CreateNewComment(Comment comment)
        {
            comment.Id = 0;
            var author = context.Users.FirstOrDefault(u => u.Id == comment.Commenter.Id);
            comment.Commenter = author;
            context.Comments.Add(comment);
            context.SaveChanges();
        }

        private List<Comment> GetAllComments()
        {
            var comments = context.Comments.Select(d => new Comment
            {
                Id = d.Id,
                DiscussionId = d.DiscussionId,
                Commenter = new IdentityUser
                {
                    // ToDo: Add test that username is not null
                    Id = d.Commenter.Id,
                    UserName = d.Commenter.UserName
                },
                Content = d.Content,
            });
            return comments.ToList();
        }

        public List<Comment> GetAllCommentsFromDiscussion(int discussionId)
        {
            var comments = GetAllComments().Where(c => c.DiscussionId == discussionId);
            return comments.ToList();
        }
        public void DeleteComment(Comment comment)
        {
            context.Comments.Remove(comment);
            context.SaveChanges();
        }
        public Comment GetCommentById(int commentId)
        {
            var comment = GetAllComments().Where(c => c.Id == commentId).First();
            return comment;
        }

        public void DeleteDiscussion(int discussionId)
        {
            context.Discussions.Remove(context.Discussions.Find(discussionId));
            context.SaveChanges();
        }

        public void EditDiscussion(Discussion editedDiscussion)
        {
            var oldDiscussion = context.Discussions.Find(editedDiscussion.Id);
            if (oldDiscussion != null)
            {
                oldDiscussion.Title = editedDiscussion.Title;
                oldDiscussion.Content = editedDiscussion.Content;
            }
            context.SaveChanges();
        }
    }
}
