using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Rental_Car.Models;

namespace Rental_Car.Controllers
{
    public class HomeController : Controller
    {
        db_rental_carEntities db = new db_rental_carEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tb_users user)
        {
            if (ModelState.IsValid)
            {
                var isEmailAlreadyExists = db.tb_users.Any(x => x.email_address == user.email_address);
                if (isEmailAlreadyExists)
                {
                    ModelState.AddModelError("email_address", "Email already exists, please enter another email.");
                    return View(user);
                }

                var isContactNumAlreadyExists = db.tb_users.Any(x => x.contact_num == user.contact_num);
                if (isContactNumAlreadyExists)
                {
                    ModelState.AddModelError("contact_num", "Contact number already exists, please enter another number.");
                    return View(user);
                }

                user.role = "user";
                db.tb_users.Add(user);
                db.SaveChanges();

                TempData["success"] = "Your account has been created successfully! Please login to continue.";
                return RedirectToAction("Login", "Home");
            }
            // else
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login_form)
        {
            if (ModelState.IsValid)
            {
                var user = db.tb_users.Where(a => a.email_address.Equals(login_form.email_address) && a.user_password.Equals(login_form.user_password)).FirstOrDefault();
                if (user != null)
                {
                    Session["user_id"] = user.user_id.ToString();
                    Session["user_name"] = user.user_name.ToString();
                    Session["role"] = user.role.ToString();

                    FormsAuthentication.SetAuthCookie(user.email_address, false);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("InvalidLoginError", "Invalid email or password!");
            }

            return View(login_form);
        }

        public ActionResult Logout()
        {
            Session["user_id"] = null;
            Session["user_name"] = null;
            Session["role"] = null;

            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Home");
        }



        public ActionResult Fleet()
        {
            var carsList = db.tb_cars.OrderBy(x  => x.rent_price).ToList();

            return View(carsList);
        }

    }
}