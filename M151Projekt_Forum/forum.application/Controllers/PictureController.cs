using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forum.business.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using forum.business.Models;
using Microsoft.AspNetCore.Identity;
using forum.business.DataAccess;

namespace forum.application.Controllers
{
    public class PictureController : Controller
    {
        private IDataAccess access;
        public PictureController(IDataAccess access)
        {
            this.access = access;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, IdentityUser author, string title, string content)

        { //id = Model.Id, author = Model.Author, title = Model.Title, content = Model.Content
            
            var fileUploadViewModel = await LoadAllPictures();
            ViewBag.Message = TempData["Message"];
            
            //access.
            //ForumDbContext context = new ForumDbContext();
            //Discussion discussion = new Discussion();
            //discussion.Id = id;
            //discussion.Author = author;
            //discussion.Title = title;
            //discussion.Content = content;
            //context.Discussions.Add(discussion);
            //context.SaveChanges();
            return View(fileUploadViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(Discussion discussion, Picture picture)

        //{ //id = Model.Id, author = Model.Author, title = Model.Title, content = Model.Content

        //    var fileUploadViewModel = await LoadAllPictures();
        //    ViewBag.Message = TempData["Message"];
        //    Discussion discussion = new Discussion();
        //    discussion.Id = id;
        //    discussion.Author = author;
        //    discussion.Title = title;
        //    discussion.Content = content;
        //    return RedirectToAction("AddDiscussion", "Discussion");
        //}

        private readonly ForumDbContext context;

        public PictureController(ForumDbContext context)
        {
            this.context = context;
        }

        public async Task<PictureUploadViewModel> LoadAllPictures()
        {
            var viewModel = new PictureUploadViewModel();
            viewModel.PicturesOnDatabase = await context.PicturesOnDatabase.ToListAsync();
            viewModel.PicturesOnFileSystem = await context.PicturesOnFileSystem.ToListAsync();
            return viewModel;
        }

        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                Discussion discussionToAddFileTo = new Discussion();
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileModel = new PictureOnFileSystemModel
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        FilePath = filePath
                    };
                    context.PicturesOnFileSystem.Add(fileModel);
                    context.SaveChanges();
                }
            }
            TempData["Message"] = "File successfully uploaded to File System.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {
            var file = await context.PicturesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {
            var file = await context.PicturesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            context.PicturesOnFileSystem.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new PictureOnDatabaseModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                context.PicturesOnDatabase.Add(fileModel);
                context.SaveChanges();
            }
            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var file = await context.PicturesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await context.PicturesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.PicturesOnDatabase.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }

    }

   
}
