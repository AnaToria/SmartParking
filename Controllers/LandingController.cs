using SmartParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartParking.Controllers
{
    public class LandingController : Controller
    {
        private SmartParkingDBEntities _entites = new SmartParkingDBEntities();

        #region Actions
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult SuccessPage(string message)
        {
            ViewBag.Message = string.Format(message);
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
        
        public ActionResult LogOut()
        {
            Session.Clear();
            return View();
        }
        #endregion

        #region HttpPost Actions
        [HttpPost]
        public ActionResult SignUp(SignUpForm form)
        {
            if (ModelState.IsValid) 
            {
                if (UserExists(form.Email))
                {
                    ViewBag.Message = string.Format("User with this email already exists");
                    return View(form);
                }
                else
                {
                    try
                    {
                        UserRegistration userRegistration = new UserRegistration
                        {
                            FirstName = form.FirstName,
                            LastName = form.LastName,
                            Phone = form.Phone,
                            Email = form.Email,
                            Password = form.Password,
                            UserType = "C",
                            Status = "A",
                            CreateDate = DateTime.Now
                        };

                        _entites.UserRegistrations.Add(userRegistration);
                        _entites.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("ErrorPage");
                    }
                    return RedirectToAction("SuccessPage", new { message = "Sign Up Went Successfully!" });
                }
            }
            else
            {
                return View(form);
            }
        }

        [HttpPost]
        public ActionResult Contact(ContactForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContactUsEmail email = new ContactUsEmail
                    {
                        SenderName = form.SenderName,
                        SenderEmail = form.SenderMail,
                        Subject = form.Subject,
                        Message = form.Message
                    };

                    _entites.ContactUsEmails.Add(email);
                    _entites.SaveChanges();
                }
                catch (Exception e)
                {
                    var temp = e;
                    return RedirectToAction("ErrorPage");
                }
                return RedirectToAction("SuccessPage", new { message = "Email Is Sent Successfully!" });
            }
            else
            {
                return View(form);
            }
        }
        
        [HttpPost]
        public ActionResult LogIn(LogInForm form)
        {
            if (ModelState.IsValid)
            {
                var user = new ActiveUser();

                user = _entites.ActiveUsers.FirstOrDefault(u => u.Email == form.Email && u.Password == form.Password);

                if (user != null)
                {
                    var info = _entites.UserRegistrations.FirstOrDefault(u => u.UserId == user.UserId);
                    Session["userId"] = user.UserId;
                    Session["userName"] = info.FirstName + " " + info.LastName;

                    if (user.UserType == "A")
                    {
                        ViewBag.Message = string.Format("User Type - Admin");
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = string.Format("User Type - Common");
                        return RedirectToAction("Dashboard","User");
                    }
                }
                else
                {
                    ViewBag.Message = string.Format("User with this creditianls do not exists. Please, check email and password.");
                    return View(form);
                }
            }
            else
            {
                return View(form);
            }
        }
        #endregion

        #region Helper Functions
        private bool UserExists(string email)
        {
            var user = new UserRegistration();
            user = _entites.UserRegistrations.FirstOrDefault(u => u.Email == email && u.Status == "A");

            return user == null ? false : true;
        }
        #endregion
    }
}