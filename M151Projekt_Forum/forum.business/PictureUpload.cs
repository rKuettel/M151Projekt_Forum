using forum.business.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AvScan.Core;
using AvScan.WindowsDefender;


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
                string defenderPath = "C:/Program Files/Windows Defender/MpCmdRun.exe";

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
                WindowsDefenderScanner fileScanner = new WindowsDefenderScanner(defenderPath);
                ScanResult scanning = fileScanner.Scan(filePath);
                string scanResult = scanning.ToString();
                if (scanning == ScanResult.ThreatFound)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        Picture pictureToDelete = pictures.Find(p => p.Name == filePath);
                        pictures.Remove(pictureToDelete);
                    }
                }
            }
            return pictures;
        }
    }
}
