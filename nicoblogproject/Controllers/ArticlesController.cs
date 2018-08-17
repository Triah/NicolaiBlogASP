using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using nicoblogproject.Models;
using nicoblogproject.Data;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nicoblogproject.Controllers
{
    [Route("Articles")]
    public class ArticlesController : Controller
    {
        private readonly UserContext _context;
        //used for ordering the articles properly
        private int articleidvalue = 1000;
        private int imagesidvalue = 100000;

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

        [Route("FileHandler")]
        public IActionResult FileHandler()
        {
            GetLoginHTMLState();
            if (HttpContext.Session.GetString("_Username") != null)
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

        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
        {
            foreach (Images image in _context.Images)
            {
                imagesidvalue--;
            }
            var filePath = Directory.GetCurrentDirectory() + "/wwwroot/Images/";
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath + file.FileName, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                Images image = new Images();
                image.ImagesID = imagesidvalue;
                image.ImagesPath = "/images/" + file.FileName;
                image.SaveDetails();
                imagesidvalue--;
            }


            return RedirectToAction("FileHandler");
        }

        [HttpPost]
        public IActionResult AddNewArticle()
        {
            string ArticleTitle = HttpContext.Request.Form["articleTitle"].ToString();
            HttpContext.Session.SetString("_ArticleTitle", ArticleTitle);

            string ArticleContent = HttpContext.Request.Form["articleContent"].ToString();
            HttpContext.Session.SetString("_ArticleContent", ArticleContent);

            if(!ArticleTitle.Equals("") && !ArticleContent.Equals(""))
            {
                return RedirectToAction("AddThumbnail");
            } else
            {
                return RedirectToAction("AddArticle");
            }
            
        }

        [HttpPost("AddThumbnailAction")]
        public IActionResult AddThumbnailAction()
        {

            foreach (Article a in _context.Articles)
            {
                articleidvalue--;
            }

            string ArticleTitle = HttpContext.Session.GetString("_ArticleTitle");
            HttpContext.Session.SetString("_ArticleTitle", "");

            string ArticleContent = HttpContext.Session.GetString("_ArticleContent");
            HttpContext.Session.SetString("_ArticleContent", "");

            string ArticleThumbnail = HttpContext.Request.Form["imagepath"].ToString();

            if (!ArticleTitle.ToString().Equals("") && !ArticleContent.ToString().Equals("")
                && !ArticleThumbnail.Equals(""))
            {
                Article article = new Article();
                article.ArticleID = articleidvalue;
                article.ArticleTitle = ArticleTitle;
                article.ArticleCreationTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                article.ArticleAuthor = HttpContext.Session.GetString("_Username").ToString();
                article.ArticleContent = ArticleContent;
                article.ArticleThumbnail = ArticleThumbnail;
                article.SaveDetails();
            }
            return RedirectToAction("Index");
        }

        [Route("AddThumbnail")]
        public IActionResult AddThumbnail()
        {
            GetLoginHTMLState();
            if (HttpContext.Session.GetString("_Username") != null)
            {
                if (HttpContext.Session.GetString("_Type").Equals("Admin"))
                {
                    List<Images> images = new List<Images>();

                    foreach (Images i in _context.Images)
                    {
                        images.Add(i);
                    }
                    return View(images);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
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
