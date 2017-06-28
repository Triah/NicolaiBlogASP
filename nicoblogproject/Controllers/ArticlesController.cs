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
    [Route("Articles")]
    public class ArticlesController : Controller
    {
        private readonly UserContext _context;
        //used for ordering the articles properly
        private int articleidvalue = 1000;

        public ArticlesController(UserContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("Index")]
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
        
        [Route("{articleTitle}")]
        [Route("(Articles/{articleTitle}")]
        public IActionResult Article(string articleTitle)
        {
            GetLoginHTMLState();
            foreach(Article a in _context.Articles)
            {
                if(a.ArticleTitle.Replace(" ", "_").Equals(articleTitle))
                {
                    ViewData["ArticleTitle"] = a.ArticleTitle;
                    ViewData["ArticleContent"] = a.ArticleContent;
                    return View();
                }
            }
            return RedirectToAction("Index");
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

            /*
             * IDEA for importing images into the article:
             * Use specialized syntax and analyse the string using a string[]
             * this specialized syntax must be unique and never used in programming
             * nor in writing for this to work
             * the code for interpreting the specialized syntax and import images into the text
             * could be using the path to the images folder in wwwroot
             * more than likely this will be difficult to implement properly but it has a definitive
             * chance of working
             * this should probably not be here
             */

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

        private void GetLoginHTMLState()
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
