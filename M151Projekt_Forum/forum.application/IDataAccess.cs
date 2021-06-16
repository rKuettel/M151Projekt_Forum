using forum.business.Models;
using System.Collections.Generic;

namespace forum.business.DataAccess
{
    public interface IDataAccess
    {
        void CreateNewDiscussion(Discussion discussion);
        List<Discussion> GetAllDiscussions();
        Discussion GetDiscussionById(int discussionId);
    }
}