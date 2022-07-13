using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private RestaurantManagementSystemEntities db = new RestaurantManagementSystemEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.ToList();
            ViewBag.Users = db.Users.ToList();
            return View(orders);
            
        }
    }
}