using forum.business.DataAccess;
using forum.business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace forum.application.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {
        private readonly IDataAccess dataAccess;

        public DiscussionController(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        //public DiscussionController()
        //{
        //    this.dataAccess = new DataAccess(new Data.ForumDbContext());
        //}

        [HttpGet]
        [Route("Discussion/Index/")]
        public IActionResult Index()
        {
            var discussions = dataAccess.GetAllDiscussions();
            return View(discussions);
        }

        [HttpGet]
        [Route("Discussion/Index/{id:int}")]
        public IActionResult Index(int id)
        {
            var discussion = dataAccess.GetDiscussionById(id);
            discussion.comments = dataAccess.GetAllCommentsFromDiscussion(id);
            return View("Discussion", discussion);
        }

        [HttpGet]
        public IActionResult AddDiscussion()
        {
            return View(new Discussion());
        }

        [HttpPost]
        public IActionResult AddDiscussion(Discussion newDiscussion)
        {
            var user = new IdentityUser();
            user.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newDiscussion.Author = user;
            dataAccess.CreateNewDiscussion(newDiscussion);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UploadPictures()
        {
            return View();
        }


    }
}
