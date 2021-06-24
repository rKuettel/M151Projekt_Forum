using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forum.business.Models;


namespace forum.application
{
    public class PictureUploadViewModel
    {
        public List<PictureOnDatabaseModel> PicturesOnDatabase { get; set; }
        public List<PictureOnFileSystemModel> PicturesOnFileSystem { get; set; }
    }
}
