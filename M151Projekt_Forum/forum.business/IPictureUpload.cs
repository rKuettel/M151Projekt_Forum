using forum.business.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace forum.business
{
    public interface IPictureUpload
    {
        Task<List<Picture>> UploadFile(List<IFormFile> files);
    }
}