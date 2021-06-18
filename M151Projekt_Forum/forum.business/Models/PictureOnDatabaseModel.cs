using System;
using System.Collections.Generic;
using System.Text;

namespace forum.business.Models
{
    public class PictureOnDatabaseModel : Picture
    {
        public byte[] Data { get; set; }
    }
}
