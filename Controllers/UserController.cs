using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rental_Car.Models;

namespace Rental_Car.Controllers
{
    public class UserController : Controller
    {
        db_rental_carEntities db = new db_rental_carEntities();

        [HttpGet]
        public ActionResult RentACar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Fleet", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult RentACar(int? id, tb_bookings booking)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Fleet", "Home");
            }

            if (ModelState.IsValid)
            {
                tb_cars car = db.tb_cars.Find(id);

                if (car == null)
                {
                    return RedirectToAction("Fleet", "Home");
                }

                booking.user_id = Convert.ToInt32(Session["user_id"]);
                booking.car_booked = id;
                booking.sub_total = car.rent_price;
                booking.booked_at = DateTime.Now;

                db.tb_bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Fleet", "Home");
            }
            return View(booking);

        }

        public ActionResult Rentals()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int user_id = Convert.ToInt32(Session["user_id"]);

            var bookings = db.tb_bookings.Where(x => x.user_id == user_id).ToList();

            return View(bookings);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int user_id = Convert.ToInt32(Session["user_id"]);
            tb_users user = db.tb_users.Find(user_id);

            user.confirm_password = user.user_password;

            return View(user);
        }


        [HttpPost]
        public ActionResult EditProfile(tb_users user_form)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int user_id = Convert.ToInt32(Session["user_id"]);
            tb_users user = db.tb_users.Find(user_id);

            user.user_name = user_form.user_name;
            user.user_password = user_form.user_password;
            user.confirm_password = user_form.confirm_password;
            user.user_address = user_form.user_address;

            db.SaveChanges();

            return View(user);
        }
    }
}