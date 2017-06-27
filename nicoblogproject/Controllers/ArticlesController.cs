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
    public class ArticlesController : Controller
    {
        private readonly UserContext _context;
        //used for ordering the articles properly
        private int articleidvalue = 1000;

        public ArticlesController(UserContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            GetLoginHTMLState();
            List<Article> articles = new List<Article>();

            foreach (Article a in _context.Articles)
            {
                articles.Add(a);
            }

            return View(articles);
        }

        [Route("addArticles")]
        public IActionResult AddArticle()
        {
            GetLoginHTMLState();
            if(HttpContext.Session.GetString("_Username") != null)
            {
                if (HttpContext.Session.GetString("_Type").Equals("Admin"))
                    {
                        return View();
                    }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult AddNewArticle()
        {
            foreach(Article a in _context.Articles)
            {
                articleidvalue--;
            }
            string ArticleTitle = HttpContext.Request.Form["articleTitle"].ToString();
            string ArticleContent = HttpContext.Request.Form["articleContent"].ToString();

            if(!ArticleTitle.ToString().Equals("") && !ArticleContent.ToString().Equals(""))
            {
                Article article = new Article();
                article.ArticleID = articleidvalue;
                article.ArticleTitle = ArticleTitle;
                article.ArticleCreationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                article.ArticleAuthor = HttpContext.Session.GetString("_Username").ToString(); 
                article.ArticleContent = ArticleContent;
                article.SaveDetails();
            }

            return RedirectToAction("Index");
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
