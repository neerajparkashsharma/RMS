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
    public class UsersController : Controller
    {
        private RestaurantManagementSystemEntities db = new RestaurantManagementSystemEntities();

        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }

        public ActionResult Active()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList().Where(x => x.IsActive == true));
        }

        // GET: Users/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Password,Email,RoleId,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy")] User user)
        {
            if (ModelState.IsValid)
            {

                user.CreatedBy = Convert.ToInt64(Session["UserId"] == null ? 0 : Session["UserId"]);
                user.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                user.IsActive = true;


                var emailMatch = db.Users.ToList().Where(x => x.Email == user.Email).ToList();


                if (emailMatch.Count() == 0)
                {

             


                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Email Already Exists!");
                }

            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.createddate = DateTime.Parse(Convert.ToString(user.CreatedDate == null ? DateTime.Now : user.CreatedDate));
            ViewBag.IsActive = user.IsActive;
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Password,Email,RoleId,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UpdatedDate = DateTime.Now;
                user.UpdatedBy = Convert.ToInt64(Session["UserId"]);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            user.IsActive = false;
            //db.Users.Remove(user);
            db.Entry(user).State = EntityState.Modified;

            db.SaveChanges();





            if (user == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            user.IsActive = false;
            //db.Users.Remove(user);
            db.Entry(user).State = EntityState.Modified;

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
