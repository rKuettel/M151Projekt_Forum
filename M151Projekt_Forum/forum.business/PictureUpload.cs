using forum.business.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace forum.business
{
    public class PictureUpload : IPictureUpload
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public PictureUpload(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<Picture>> UploadFile(List<IFormFile> files)
        {
            List<IFormFile> fileList = files;
            List<Picture> pictures = new List<Picture>();
            foreach (IFormFile file in fileList)
            {
                string fileGuid = Guid.NewGuid().ToString();
                string directory = hostingEnvironment.WebRootPath;
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
                        pictures.Add(new Picture
                        {
                            Name = fileName,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
            return pictures;
        }
    }
}
