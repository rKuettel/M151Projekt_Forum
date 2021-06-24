using forum.business.Models;
using System.Collections.Generic;

namespace forum.business.DataAccess
{
    public interface IDataAccess
    {
        void CreateNewDiscussion(Discussion discussion);
        public void CreateNewComment(Comment comment);
        List<Discussion> GetAllDiscussions();
        Discussion GetDiscussionById(int discussionId);
        List<Comment> GetAllCommentsFromDiscussion(int discussionId);

        void DeleteComment(Comment comment);
        Comment GetCommentById(int commentId);
        void DeleteDiscussion(int discussionId);
        public void EditDiscussion(Discussion editedDiscussion);
    }
}