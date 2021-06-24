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
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using forum.business;

namespace forum.application.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {
        private readonly IDataAccess dataAccess;
        private readonly IPictureUpload pictureUpload;

        public DiscussionController(IDataAccess dataAccess, IPictureUpload pictureUpload)
        {
            this.dataAccess = dataAccess;
            this.pictureUpload = pictureUpload;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Discussion/Index/")]
        public IActionResult Index()
        {
            var discussions = dataAccess.GetAllDiscussions();
            return View(discussions);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Discussion/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var discussion = dataAccess.GetDiscussionById(id);
            discussion.Comments = dataAccess.GetAllCommentsFromDiscussion(id);
            return View("Discussion", discussion);        
        }

        [HttpGet]
        public IActionResult AddDiscussion()
        {
            return View(new Discussion());
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscussion(Discussion newDiscussion, List<Microsoft.AspNetCore.Http.IFormFile> files)
        {
            var user = new IdentityUser();
            user.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newDiscussion.Author = user;

            newDiscussion.Pictures = await pictureUpload.UploadFile(files);

            dataAccess.CreateNewDiscussion(newDiscussion);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditDiscussion(int id) 
        {
            Discussion discussion = dataAccess.GetDiscussionById(id);
            if (discussion.Author.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return View(discussion);
            }
            return RedirectToAction("Details", id);
            throw new UnauthorizedAccessException("You are not allowed to do this action");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDiscussion(Discussion editedDiscussion)
        {
            if (editedDiscussion.Author.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                dataAccess.EditDiscussion(editedDiscussion);
                return RedirectToAction("Index", editedDiscussion.Id);
            }

            return View("Unauthorized");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Discussion/DeleteDiscussion/")]
        public IActionResult DeleteDiscussion(int id)
        {
            var discussion  = dataAccess.GetDiscussionById(id);
            if (discussion.Author.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                dataAccess.DeleteDiscussion(id);
                return RedirectToAction("Index");
            };
            return View("Unauthorized");
        }
    }
}
