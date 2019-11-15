using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyIdentityService.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult UserProfile(int id)
        {
            return View();
        }

        public IActionResult Settings(int id)
        {
            return View();
        }
    }
}