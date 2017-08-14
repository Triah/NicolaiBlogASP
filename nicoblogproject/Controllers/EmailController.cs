using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit.Net.Imap;
using nicoblogproject.Data;
using nicoblogproject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nicoblogproject.Controllers
{
    public class EmailController : Controller
    {

        private readonly UserContext _context;

        public EmailController(UserContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            GetLoginHTMLState();
            if (HttpContext.Session.GetString("_Username") != null)
            {
                if (HttpContext.Session.GetString("_Type").Equals("Admin"))
                {
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult SendNewsletter()
        {
            string FromAdress = "nicolaihedegaardjensen93@gmail.com";
            string FromAdressTitle = "Realm of Code: Newsletter " + DateTime.Today.ToString("dd-MM-yyyy");
            string ToAdressTitle = "Email Blog";
            string Subject = HttpContext.Request.Form["subjectNewsletter"].ToString();
            string BodyContent = HttpContext.Request.Form["contentNewsletter"].ToString(); 
            string SmtpServer = "smtp.gmail.com";
            int SmtpPortNumber = 587;

            foreach(var el in _context.EmailList) {
                string ToAdress = el.Email;
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAdress));
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAdress));
                mimeMessage.Subject = Subject;
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, ch, e) => true;
                    client.Connect(SmtpServer, SmtpPortNumber, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("nicolaihedegaardjensen93@gmail.com", "hiddenpassword");
                    client.Send(mimeMessage);


                    client.Disconnect(true);
                }
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
