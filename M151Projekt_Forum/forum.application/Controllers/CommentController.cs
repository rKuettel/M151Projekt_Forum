using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using forum.business.Models;
using forum.business.DataAccess;

namespace forum.application.Controllers
{
    public class CommentController : Controller
    {
        private readonly IDataAccess dataAccess;

        public CommentController(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        [Route("Comment/Index/{id:int}")]
        public IActionResult Index(int discussionId)
        {
            var comments = dataAccess.GetAllCommentsFromDiscussion(discussionId);
            return View(comments);
        }

        [HttpPost]
        [Route("Comment/AddComment/{discussionId:int}")]
        public IActionResult AddComment(Comment newComment)
        {
            var user = new IdentityUser();
            user.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newComment.Commenter = user;
            dataAccess.CreateNewComment(newComment);
            return RedirectToAction("Details", "Discussion", new { id = newComment.DiscussionId });
        }

        [HttpPost]
        [Route("Comment/DeleteComment/{commentId:int}")]
        public IActionResult DeleteComment(int commentId)
        {
            
            Comment comment = dataAccess.GetCommentById(commentId);

            if (comment.Commenter.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                dataAccess.DeleteComment(comment);
                return RedirectToAction("Details", "Discussion", new { id = comment.DiscussionId });
            };
            return View("Unauthorized");         
        }
    }
}
