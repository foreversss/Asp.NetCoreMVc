using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreMVC.Controllers
{
    public class ThirdController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
