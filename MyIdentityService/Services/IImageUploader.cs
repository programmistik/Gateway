using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Services
{
    public interface IImageUploader
    {
        Task<string> Upload(IFormFile file);
    }
}
