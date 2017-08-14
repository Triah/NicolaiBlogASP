using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nicoblogproject.Data;
using nicoblogproject.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nicoblogproject.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserContext _context;
        const string SessionKeyRegisterName = "_RegisterUsername";
        const string SessionKeyRegisterEmail = "_RegisterEmail";
        const string SessionKeyRegisterPassword = "_RegisterPassword";
        const string SessionKeyErrorOccured = "_Error";
        const string SessionLoggedIn = "_LoggedInVariable";
        const string SessionLoggedInTrue = "_LoggedInTrue";
        const string SessionLoggedInFalse = "_LoggedInFalse";
        const string SessionUsername = "_Username";
        const string SessionEmail = "_Email";
        const string SessionUserType = "_Type";
        const string SessionKeyAcceptTerms = "_TermsOfService";
        const string SessionUserSalt = "_Salt";

        public LoginController(UserContext context)
        {
            _context = context;
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

        // GET: /<controller>/
        public IActionResult Index()
        {
            GetLoginHTMLState();

            if (HttpContext.Session.GetString(SessionLoggedIn) != null)
            {
                if (HttpContext.Session.GetString(SessionLoggedIn).ToString().Equals(SessionLoggedInTrue))
                {
                    return RedirectToAction("Profile");
                }
            }

            if (HttpContext.Session.GetString(SessionLoggedIn) == null)
            {
                HttpContext.Session.SetString(SessionLoggedIn, SessionLoggedInFalse);
            }

            //cookie data
            if (HttpContext.Session.GetString(SessionKeyRegisterName) != null &&
                HttpContext.Session.GetString(SessionKeyRegisterEmail) != null &&
                HttpContext.Session.GetString(SessionKeyRegisterPassword) != null)
            {
                var RegisterUsername = HttpContext.Session.GetString(SessionKeyRegisterName);
                HttpContext.Session.SetString(SessionKeyRegisterName, "");

                var RegisterEmail = HttpContext.Session.GetString(SessionKeyRegisterEmail);
                HttpContext.Session.SetString(SessionKeyRegisterEmail, "");

                var RegisterPassword = HttpContext.Session.GetString(SessionKeyRegisterPassword);
                HttpContext.Session.SetString(SessionKeyRegisterPassword, "");

                var RegisterSalt = HttpContext.Session.GetString(SessionUserSalt);
                HttpContext.Session.SetString(SessionUserSalt, "");

                if (!RegisterUsername.ToString().Equals("") &&
                    !RegisterEmail.ToString().Equals("") &&
                    !RegisterPassword.ToString().Equals("") &&
                    !RegisterSalt.ToString().Equals(""))
                {
                    ApplicationUser applicationUser = new ApplicationUser();
                    applicationUser.ApplicationUserID = Guid.NewGuid().GetHashCode();
                    applicationUser.Username = RegisterUsername;
                    applicationUser.Email = RegisterEmail;
                    applicationUser.Salt = RegisterSalt;
                    applicationUser.Password = RegisterPassword;
                    applicationUser.Type = "Basic";
                    applicationUser.SaveDetails();
                }

            }

            if (HttpContext.Session.GetString(SessionLoggedIn).ToString().Equals(SessionLoggedInTrue)) {
                return RedirectToAction("Profile");
            }

            if (HttpContext.Session.GetString(SessionKeyErrorOccured) != null)
            {
                ViewData["LoginError"] = HttpContext.Session.GetString(SessionKeyErrorOccured);
            }

            return View();
        }

        public string CreateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string EncryptPassword( string password, byte[] salt)
        {
            string EncPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA1, iterationCount: 10000, numBytesRequested: 256 / 8));
            return EncPassword;
        }
        

        [Route("profile")]
        public IActionResult Profile()
        {
            GetLoginHTMLState();
            if (HttpContext.Session.GetString(SessionLoggedIn) != null)
            {
                if (HttpContext.Session.GetString(SessionLoggedIn).ToString().Equals(SessionLoggedInFalse))
                {
                    return RedirectToAction("Index");
                }
            }
            //Display Username and Email based on Session cookie
            ViewData["Username"] = HttpContext.Session.GetString(SessionUsername);
            ViewData["Email"] = HttpContext.Session.GetString(SessionEmail);

            return View();
        }

        [HttpPost]
        public IActionResult Authenticate()
        {
            string LoginUsername = HttpContext.Request.Form["loginUsername"].ToString();
            string LoginPassword = HttpContext.Request.Form["loginPassword"].ToString();
            foreach (ApplicationUser au in _context.Users)
            {
                if (LoginUsername.ToString().Equals(au.Username.ToString(), StringComparison.CurrentCultureIgnoreCase) && EncryptPassword(LoginPassword, Convert.FromBase64String(au.Salt)) == au.Password)
                {
                    HttpContext.Session.SetString(SessionUsername, au.Username);
                    HttpContext.Session.SetString(SessionEmail, au.Email);
                    HttpContext.Session.SetString(SessionLoggedIn, SessionLoggedInTrue);
                    HttpContext.Session.SetString(SessionUserType, au.Type);
                    return RedirectToAction("Profile");
                }
            }
            HttpContext.Session.SetString(SessionKeyErrorOccured,
                        "Username or Password is incorrect");
            return RedirectToAction("Index");
        }

        [Route("register")]
        public IActionResult Register()
        {
            GetLoginHTMLState();
            if (HttpContext.Session.GetString(SessionLoggedIn) != null)
            {
                if (HttpContext.Session.GetString(SessionLoggedIn).ToString().Equals(SessionLoggedInTrue))
                {
                    return RedirectToAction("Profile");
                }
            }
            if (HttpContext.Session.GetString(SessionKeyErrorOccured) != null)
            {
                ViewData["ErrorMessage"] = HttpContext.Session.GetString(SessionKeyErrorOccured);
            }
            return View();
        }

        /**
         * This method is used to get the form data and validate the input
         * prevents SQL injections and such attacks
         **/
        [HttpPost]
        public IActionResult GetUserData()
        {
            //get data from request forms
            string Username = HttpContext.Request.Form["registerUsername"].ToString();
            string Email = HttpContext.Request.Form["registerEmail"].ToString();
            string Password = HttpContext.Request.Form["registerPassword"].ToString();
            string ConfirmPassword = HttpContext.Request.Form["confirmPassword"].ToString();
            string SignupNewsletter = HttpContext.Request.Form["registerNewsletterCheckbox"].ToString();
            string SignupTermsOfService = HttpContext.Request.Form["registerTermsOfServiceCheckbox"].ToString();

            //input checks

            //check if Username is empty
            if (Username.Equals(""))
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                       "Username may not be empty");
                return RedirectToAction("Register");
            }
            //check for special characters
            else if (Username.IndexOf("(", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                Username.IndexOf(")", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                Username.IndexOf(";", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                Username.IndexOf("=", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                          "Username contains illegal characters");
                return RedirectToAction("Register");
            }
            //check if Email is empty
            else if (Email.Equals(""))
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                       "Email may not be empty");
                return RedirectToAction("Register");
            }
            //ensure Email has a @ char
            else if (Email.IndexOf("@", StringComparison.CurrentCultureIgnoreCase) == -1)
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                    "Email must contain @");
                return RedirectToAction("Register");
            }
            //ensure email has a . char
            else if (Email.IndexOf(".", StringComparison.CurrentCultureIgnoreCase) == -1)
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                    "Email must have a domain");
                return RedirectToAction("Register");
            }
            //ensure there are no illegal chars in email
            else if (Email.IndexOf("(", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                Email.IndexOf(")", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                Email.IndexOf(";", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                Email.IndexOf("=", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                          "Email contains illegal characters");
                return RedirectToAction("Register");
            }
            //ensure password is longer than 7 chars
            else if (Password.Length < 7)
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                    "Password must be atleast 8 characters");
                return RedirectToAction("Register");
            }
            //ensure password are no illegal chars in password
            else if (Password.IndexOf("(", StringComparison.CurrentCultureIgnoreCase) != -1 ||
               Password.IndexOf(")", StringComparison.CurrentCultureIgnoreCase) != -1 ||
               Password.IndexOf(";", StringComparison.CurrentCultureIgnoreCase) != -1 ||
               Password.IndexOf("=", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                          "Email contains illegal characters");
                return RedirectToAction("Register");
            }
            //ensure password and confirmation password match
            else if (!Password.Equals(ConfirmPassword))
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured,
                    "Passwords do not match");
                return RedirectToAction("Register");
            }

            else if (SignupTermsOfService.Equals(""))
            {
                HttpContext.Session.SetString(SessionKeyErrorOccured, "You must accept the Terms of Service");
                return RedirectToAction("Register");
            }

            foreach (ApplicationUser a in _context.Users)
            {
                if (a.Username.ToString().Equals(Username, StringComparison.CurrentCultureIgnoreCase))
                {
                    HttpContext.Session.SetString(SessionKeyErrorOccured,
                        "Username is already in use");
                    return RedirectToAction("Register");
                }
                else if (a.Email.ToString().Equals(Email,StringComparison.CurrentCultureIgnoreCase))
                {
                    HttpContext.Session.SetString(SessionKeyErrorOccured,
                        "Email is already in use");
                    return RedirectToAction("Register");
                }
            }

            //store values using sessions
            HttpContext.Session.SetString(SessionKeyRegisterName, Username);
            HttpContext.Session.SetString(SessionKeyRegisterEmail, Email);
            HttpContext.Session.SetString(SessionUserSalt, CreateSalt());
            HttpContext.Session.SetString(SessionKeyRegisterPassword, EncryptPassword(Password, Convert.FromBase64String(HttpContext.Session.GetString(SessionUserSalt))));
            

            if (SignupNewsletter.Equals("on"))
            {
                EmailList el = new EmailList();
                el.EmailListID = Guid.NewGuid().GetHashCode();
                el.Email = Email;
                if (el.Email.ToString().IndexOf("@") != -1 && el.Email.ToString().IndexOf(".") != -1
                    && !el.Email.Equals("") && el.Email.ToString().IndexOf(";") == -1
                    && el.Email.ToString().IndexOf("(") == -1 && el.Email.ToString().IndexOf(")") == -1
                    && el.Email.ToString().IndexOf(",") == -1 && el.Email.ToString().IndexOf("'") == -1
                    && el.Email.ToString().IndexOf(":") == -1)
                {
                    foreach (var email in _context.EmailList)
                    {
                        if (el.Email.ToString().Equals(email.Email))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    el.SaveDetails();
                }
            }
            else
            {
            }

            return RedirectToAction("Index");
            }
        


        [Route("forgotpassword")]
        public IActionResult ForgotPassword()
        {
            GetLoginHTMLState();

            if (HttpContext.Session.GetString(SessionLoggedIn) != null)
            {
                if (HttpContext.Session.GetString(SessionLoggedIn).ToString().Equals(SessionLoggedInTrue))
                {
                    return RedirectToAction("Profile");
                }
            }

            return View();
        }

        [Route("termsofservice")]
        public IActionResult TermsOfService()
        {
            GetLoginHTMLState();
            return View();
        }

        [Route("privacypolicy")]
        public IActionResult PrivacyPolicy()
        {
            GetLoginHTMLState();
            return View();
        }

    }
}
