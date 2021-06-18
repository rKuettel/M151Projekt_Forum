using System;
using System.Collections.Generic;
using System.Text;

namespace forum.business.Models
{
    public class PictureOnFileSystemModel : Picture
    {
        public string FilePath { get; set; }
    }
}
