using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RMS.Controllers
{
    public class AccountController : Controller
    {

        RestaurantManagementSystemEntities rms = new RestaurantManagementSystemEntities();


        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {



            if ( Email != null && Password != null  )
            {


               var list = rms.Users.ToList().Where(x => x.Email.ToLower() == Email && x.Password == Password && x.IsActive == true).FirstOrDefault();


                   

                if (list != null)
                {
                    FormsAuthentication.SetAuthCookie(Email, false);


                    Session["UserId"] = list.Id;
                    Session["Name"] = list.Name;
                    Session["RoleId"] = list.RoleId;
                   
                    Session["Email"] = list.Email;

                    Session["IsActive"] = list.IsActive;
                    
                   // Session["UserList"] = rms.Users.ToList();


                    return RedirectToAction("Index", "Users");
                }

                else
                {
                    ModelState.AddModelError("", "invalid Email or Password");

                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session["UserId"] ="";
            Session["Name"] = "";
            Session["RoleId"] = "";
            
            Session["Email"] = "";

            Session["IsActive"] = "";

            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}