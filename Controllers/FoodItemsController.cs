using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
    [Authorize]
    public class FoodItemsController : Controller
    {
        private RestaurantManagementSystemEntities db = new RestaurantManagementSystemEntities();

        
       
        // GET: FoodItems
        public ActionResult Index()
        {
            return View(db.FoodItems.ToList().Where(x=>x.IsActive==true));
        }

        public ActionResult Pizzas()
        {
            return View(db.SelectAllPizza().ToList());
        }
        public ActionResult Burgers()
        {
            return View(db.SelectAllBurger().ToList());
        }

        public ActionResult FrenchFries()
        {
            return View(db.SelectAllFrenchFries().ToList());
        }

        // GET: FoodItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodItem foodItem = db.FoodItems.Find(id);
            if (foodItem == null)
            {
                return HttpNotFound();
            }
            return View(foodItem);
        }

        // GET: FoodItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,IsActive,CategoryId,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,Image")] FoodItem foodItem, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                foodItem.CreatedBy = Convert.ToInt64(Session["UserId"] == null ? 0 : Session["UserId"]);
                foodItem.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                foodItem.IsActive = true;

                if (file != null && file.ContentLength > 0)
                {
                    // Save Object User



                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    file.SaveAs(physicalPath);





                    foodItem.Image = ImageName;


                }
                    db.FoodItems.Add(foodItem);
                db.SaveChanges();
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", foodItem.CategoryId);
                return RedirectToAction("Index");
            }

            return View(foodItem);
        }

        // GET: FoodItems/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodItem foodItem = db.FoodItems.Find(id);
            if (foodItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", foodItem.CategoryId);
            return View(foodItem);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,IsActive,CategoryId,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,Image")] FoodItem foodItem, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                foodItem.UpdatedBy = Convert.ToInt64(Session["UserId"] == null ? 0 : Session["UserId"]);
                foodItem.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                


                if (file != null && file.ContentLength > 0)
                {

                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    file.SaveAs(physicalPath);





                    foodItem.Image = ImageName;
                }
                ViewBag.createddate = DateTime.Parse(Convert.ToString(foodItem.CreatedDate == null ? DateTime.Now : foodItem.CreatedDate));
                db.Entry(foodItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", foodItem.CategoryId);
            return View(foodItem);
        }

        // GET: FoodItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodItem foodItem = db.FoodItems.Find(id);
            foodItem.IsActive = false;
            db.SaveChanges();
            if (foodItem == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
            }

        // POST: FoodItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            FoodItem foodItem = db.FoodItems.Find(id);
            foodItem.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
