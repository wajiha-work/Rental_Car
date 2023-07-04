using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rental_Car.Models;

namespace Rental_Car.Controllers
{
    public class AdminController : Controller
    {
        db_rental_carEntities db = new db_rental_carEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddCar()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddCar(tb_cars car, HttpPostedFileBase customFile)
        {
            if (ModelState.IsValid)
            {
                var isCarExists = db.tb_cars.Any(x => x.car_name.Equals(car.car_name));
                if (isCarExists)
                {
                    ModelState.AddModelError("car_name", "Car name already exists!");
                    return View(car);
                }


                Dictionary<string, string> fileResult = UploadImage(customFile);
                if (fileResult.ContainsKey("success")) // image uploaded
                {
                    car.picture = fileResult["success"]; // will save path of image

                    db.tb_cars.Add(car);
                    db.SaveChanges();
                    TempData["success"] = "Car added successfully!";
                    return RedirectToAction("CarsList", "Admin");
                }
                else // image not uploaded
                {
                    TempData["error"] = fileResult["error"];
                    return View(car);
                }
            }

            return View(car);
        }


        public ActionResult CarsList()
        {
            var cars = db.tb_cars.ToList();

            return View(cars);
        }

        [HttpGet]
        public ActionResult ViewCar(int? id)
        {
            tb_cars car = db.tb_cars.Find(id);

            if (car == null)
            {
                TempData["error"] = "No car found!";
                return RedirectToAction("CarsList", "Admin");
            }

            return View(car);
        }

        [HttpGet]
        public ActionResult EditCar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("CarsList", "Admin");
            }
            tb_cars car = db.tb_cars.Find(id);
            if (car == null)
            {
                return RedirectToAction("CarsList", "Admin");
            }
            return View(car);
        }

        [HttpPost]
        public ActionResult EditCar(int? id, tb_cars car_form, HttpPostedFileBase customFile)
        {
            if (ModelState.IsValid)
            {
                tb_cars car = db.tb_cars.Find(id);

                if (car == null)
                {
                    TempData["error"] = "No car found!";
                    return RedirectToAction("CarsList", "Admin");
                }

                car_form.picture = car.picture;

                car.car_name = car_form.car_name;
                car.model = car_form.model;
                car.make = car_form.make;
                car.rent_price = car_form.rent_price;
                car.description = car_form.description;

                if (customFile == null) // if file is not added
                {
                    db.SaveChanges();
                    TempData["success"] = "Car Updated succesfully";
                    return RedirectToAction("CarsList", "Admin");
                }
                else // if file is added
                {
                    Dictionary<string, string> fileResult = UploadImage(customFile, car.picture);

                    if (fileResult.ContainsKey("success"))
                    {
                        car.picture = fileResult["success"];
                        car_form.picture = fileResult["success"]; // for preview

                        db.SaveChanges();
                        TempData["success"] = "Car Updated succesfully";
                        return RedirectToAction("CarsList", "Admin");
                    }
                    else
                    {
                        TempData["error"] = fileResult["error"];
                    }
                }
            }

            return View(car_form);
        }

        [HttpGet]
        public ActionResult DeleteCar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("CarsList", "Admin");
            }
            tb_cars car = db.tb_cars.Find(id);
            if (car == null)
            {
                return RedirectToAction("CarsList", "Admin");
            }
            return View(car);
        }

        [HttpPost, ActionName("DeleteCar")]
        public ActionResult ConfirmDeleteCar(int id)
        {
            tb_cars car = db.tb_cars.Find(id);
            db.tb_cars.Remove(car);
            db.SaveChanges();
            TempData["success"] = "Car deleted successfully!";
            return RedirectToAction("CarsList", "Admin");
        }







        // image upload
        private bool DeleteImage(string filename)
        {
            bool result = false;
            if (isFileALreadyExist(filename))
            {
                try
                {
                    string path = Server.MapPath(filename);
                    FileInfo file = new FileInfo(path);
                    file.Delete();
                    result = true;
                }
                catch (System.IO.IOException ex)
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool isFileALreadyExist(string filename)
        {
            string path = Server.MapPath(filename);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Dictionary<string, string> UploadImage(HttpPostedFileBase file, string productFileName = "")
        {
            Dictionary<string, string> result = new Dictionary<string, string>(); // output variable

            if (file != null && file.ContentLength > 0) // if file IS sent here
            {
                string filename = Path.GetFileName(file.FileName); // get filename
                filename = "/UploadedFiles/" + filename; // set prefix for our uploads folder

                if (isFileALreadyExist(filename)) // if file sent already exist w.r.t. name
                {
                    if (filename == productFileName) // exist but it is exactly same file choosen which is saved in db so no need to upload new and delete old one - just return
                    {
                        result.Add("success", filename);
                        return result;
                    }
                    else // in any other case where file exists with same name, either add or edi -> give error
                    {
                        result.Add("error", "Another file with same name already exists, please rename your file and upload again!");
                        return result;
                    }
                }

                string extension = Path.GetExtension(file.FileName);

                // not works if extension is jpeg, jgp or png
                if (extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        // set unique file name
                        // string filename = Path.GetFileNameWithoutExtension(file.FileName);
                        // filename += "_" + DateTime.Now.ToString("yymmssfff") + extension;

                        // map with local path w.r.t drive and save image there
                        string path = Server.MapPath(filename);
                        file.SaveAs(path);

                        // in case of edit, delete old file too
                        if (productFileName != "")
                        {
                            DeleteImage(productFileName);
                        }

                        result.Add("success", filename);
                    }
                    catch (Exception ex)
                    {
                        //result.Add("error", "Some unknown error occured while uploading the file!");
                        result.Add("error", ex.Message);
                    }
                }
                else
                {
                    result.Add("error", "Only JPG,JPEG and PNG formats are acceptable!");
                }
            }
            else
            {
                result.Add("error", "Please upload product image!");
            }
            return result;
        }

    }
}