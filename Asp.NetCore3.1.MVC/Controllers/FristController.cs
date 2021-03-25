using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore3._1.MVC.Controllers
{
    public class FristController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(base.HttpContext.Session.GetString("")))
            {

            }


            return View();
        }
    }
}
