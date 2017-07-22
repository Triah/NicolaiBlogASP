using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using nicoblogproject.Models;
using nicoblogproject.Data;

namespace nicoblogproject.Controllers
{
    public class HomeController : Controller
    {
        const string NewsletterResult = "NewsletterSignupResult";
        private readonly UserContext _context;

        public HomeController(UserContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            GetLoginHTMLState();
            if (HttpContext.Session.GetString(NewsletterResult) != null)
            {
                ViewData["NewsletterResult"] = HttpContext.Session.GetString(NewsletterResult);
                HttpContext.Session.SetString(NewsletterResult, "");
            }
                return View();
        }

        public IActionResult Error()
        {
            GetLoginHTMLState();

            return View();
        }

        [HttpPost]
        public IActionResult NewsletterRegistration()
        {
            //Code needed for adding emails to the email list
            EmailList el = new EmailList();
            el.EmailListID = Guid.NewGuid().GetHashCode();
            el.Email = HttpContext.Request.Form["newsletterRegister"].ToString();
            if(el.Email.ToString().IndexOf("@")!=-1 && el.Email.ToString().IndexOf(".")!=-1
                && !el.Email.Equals("") && el.Email.ToString().IndexOf(";")==-1
                && el.Email.ToString().IndexOf("(") == -1 && el.Email.ToString().IndexOf(")") == -1
                && el.Email.ToString().IndexOf(",") == -1 && el.Email.ToString().IndexOf("'") == -1
                && el.Email.ToString().IndexOf(":") == -1) 
            {
                foreach(var email in _context.EmailList)
                {
                    if (el.Email.ToString().Equals(email.Email))
                    {
                        HttpContext.Session.SetString(NewsletterResult, "Subscription failed, email is already subscribed");
                        return RedirectToAction("Index");
                    }
                }
                el.SaveDetails();
                HttpContext.Session.SetString(NewsletterResult, "Subscription Successful");
                return RedirectToAction("Index");
            } else
            {
                HttpContext.Session.SetString(NewsletterResult, "Subscription failed, please ensure your email is correct");
                return RedirectToAction("Index");
            }       
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
