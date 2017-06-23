using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace nicoblogproject.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            GetLoginHTMLState();
            
            return View();
        }

        public IActionResult Error()
        {
            GetLoginHTMLState();

            return View();
        }

        public void GetLoginHTMLState()
        {
            if (HttpContext.Session.GetString("_LoggedInVariable") != null)
            {
                if (HttpContext.Session.GetString("_LoggedInVariable").ToString().Equals("_LoggedInTrue"))
                {
                    ViewData["LoginOrProfile"] = "Profile";
                }
                else
                {
                    ViewData["LoginOrProfile"] = "Login";
                }
            }
            else
            {
                ViewData["LoginOrProfile"] = "Login";
            }
        }
    }
}
