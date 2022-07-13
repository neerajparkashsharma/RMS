using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        private List<Cart> ListCarts;
        public CheckOutController()
        {
            ListCarts = new List<Cart>();

        }


        

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckoutOrder(Order obj)
        {



            try
            {
          

                foreach (var item in Session["Cart"] as List<Cart>)
                {
                    using (RestaurantManagementSystemEntities db = new RestaurantManagementSystemEntities())
                    {

                        Order ord = new Order();

                        ord.Amount = obj.Amount;
                        ord.CreatedBy = Convert.ToInt64(Session["UserId"]); 
                        ord.CreatedDate = DateTime.Now;
                        ord.IsActive = true;
                        db.Orders.Add(ord);
                        db.SaveChanges();


                        OrderDetail od = new OrderDetail();
                        od.OrderId = ord.Id;
                        od.FoodItemId = item.FoodItemId;
                        od.Quantity = item.Quantity;
                        od.CreatedBy = Convert.ToInt64(Session["UserId"]);
                        od.CreatedDate = DateTime.Now;
                        db.OrderDetails.Add(od);

                        db.SaveChanges();
                    }
                }
               
                Session["Cart"] = null;
               
            }
            catch (Exception ex)
            {
                var exe = ex;
                return View();
            }


            return RedirectToAction("Index", "Orders");


        }
        public ActionResult Cart()
        {
            return View();
        }

        private RestaurantManagementSystemEntities db = new RestaurantManagementSystemEntities();

        [HttpPost]
        public ActionResult Cart(long Id)
        {

            long Quantity = 1;

        var list = db.FoodItems.Where(x=>x.Id == Id).FirstOrDefault();

            Cart cart = new Cart();
            cart.FoodItemId = list.Id;
            cart.Name = list.Name;
            cart.Price = list.Price;
            cart.Quantity = Quantity;
            cart.Image = list.Image;
            cart.bill = cart.Price == null ? 0 : cart.Price.Value * cart.Quantity;


            if (Session["Cart"] == null)
            {
                List<Cart> li = new List<Cart>();

                li.Add(cart);
                Session["Cart"] = li;

                ViewBag.Cart = li.Count();






            }
            else
            {
                List<Cart> li2 = Session["Cart"] as List<Cart>;


                int flag = 0;

                foreach (var item in li2)
                {
                    if (item.FoodItemId == cart.FoodItemId)
                    {
                        item.Quantity += cart.Quantity;
                        item.bill += cart.bill;
                        flag = 1;
                    }
                }

                if (flag == 0)
                {
                    li2.Add(cart);
                }


                Session["Cart"] = li2;
                ViewBag.Cart = li2;



            }
            long count = 0;
            foreach (var item in Session["Cart"] as List<Cart>)
            {
                count = count + item.Quantity;
                Session["Count"] = count;

            }

            Session["Count"] = count;

            return RedirectToAction("Cart");
        }


        public ActionResult RemoveFromCart(int foodItemId)
        {
            List<Cart> cart = (List<Cart>)Session["Cart"];
            foreach (var item in cart)
            {
                if (item.FoodItemId == foodItemId)
                {
                    cart.Remove(item);
                    break;
                }
            }

            long count = 0;
            foreach (var item in Session["Cart"] as List<Cart>)
            {
                count = count + item.Quantity;


            }
            Session["Cart"] = cart;
            Session["Count"] = count;
            return Redirect("Cart");
        }





    }
}