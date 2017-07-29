using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using nicoblogproject.Models;
using nicoblogproject.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nicoblogproject.Controllers
{
    public class CommunityController : Controller
    {

        private readonly UserContext _context;
        const string SearchFilter = "_SearchFilter";

        public CommunityController(UserContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            GetLoginHTMLState();
            if(HttpContext.Session.GetString(SearchFilter) != null &&
                !HttpContext.Session.GetString(SearchFilter).Equals(""))
            {
                string _SearchFilter = HttpContext.Session.GetString(SearchFilter);
                foreach(ApplicationUser user in _context.Users)
                {
                    if (user.Username.Equals(_SearchFilter, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (users.Count != 0)
                        {
                            users.Clear();
                        }
                        users.Add(user);
                        HttpContext.Session.SetString(SearchFilter, "");
                        return View(users);
                    }
                    if(user.Username.IndexOf(_SearchFilter, StringComparison.CurrentCultureIgnoreCase) != -1 
                        && !users.Contains(user))
                    {
                        users.Add(user);
                    }
                }
            }
            else
            {
                foreach (ApplicationUser user in _context.Users)
                {
                    users.Add(user);
                }
            }

            
            return View(users);
        }


        //search bar function basic is down, gotta make profiles before i can keep going
        [HttpPost("SearchByUsername")]
        public IActionResult SearchByUsername()
        {
            string SearchQuery = HttpContext.Request.Form["communitySearchBar"].ToString();
            HttpContext.Session.SetString(SearchFilter, SearchQuery);

            return RedirectToAction("Index");
        }

        /*
         * 
         * this is for testing the images for the community tab
        
            [HttpPost("AddImageToDb")]
        public IActionResult AddImageToDb()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.ApplicationUserID = Guid.NewGuid().GetHashCode();
            applicationUser.Username = "imagetest";
            applicationUser.Email = "imageemail@email.em";
            applicationUser.Password = "lolthisisatest";
            applicationUser.DisplayImage = "images/userimages/airadventurelevel1.png";
            applicationUser.Type = "Basic";
            applicationUser.SaveDetails();

            return RedirectToAction("Index");
        }
        */

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
