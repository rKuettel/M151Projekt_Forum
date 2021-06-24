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

namespace forum.application.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IDataAccess dataAccess;

        public DiscussionController(IDataAccess dataAccess, IHostingEnvironment hostingEnvironment)
        {
            this.dataAccess = dataAccess;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        [Route("Discussion/Index/")]
        public IActionResult Index()
        {
            var discussions = dataAccess.GetAllDiscussions();
            return View(discussions);
        }

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
            
            
            List<IFormFile> fileList = files;
            newDiscussion.Pictures = new List<Picture>();
            foreach (IFormFile file in fileList)
            {
                string fileGuid = Guid.NewGuid().ToString();
                string directory = _hostingEnvironment.WebRootPath;
                var basePath = Path.Combine(directory + "\\Files\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = fileGuid + fileExtension;
                var filePath = Path.Combine(basePath, fileName);
                
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        newDiscussion.Pictures.Add(new Picture
                        {
                            DiscussionId = newDiscussion.Id,
                            Name = fileName,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
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

        [HttpGet]
        public IActionResult UploadPictures()
        {
            return View();
        }


    }
}
