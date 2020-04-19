using MyIdentityService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.ViewModels
{
    public class VueViewModel
    {
        [VueData("message")]
        public string Message { get; set; } = "Hello from Vue!";

        [VueData("menu")]
        public List<string> MenuItems { get; set; } = new List<string>()
        {
        "Menu 1",
        "Menu 2",
        };

        public string RazorMessage { get; set; } = "Hello from Razor!";

        // in a real app, this would be placed in the base view model class
        public Dictionary<string, object> VueData { get; set; } = new Dictionary<string, object>();
    }
}
