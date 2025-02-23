using StackOverFlowProject.CustomFilters;
using StackOverFlowProject.ServiceLayer;
using StackOverFlowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlowProject.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                int uId = _userService.InsertUser(rvm);
                Session["CurrentUserId"] = uId;
                Session["CurrentUserName"] = rvm.Name;
                Session["CurrentUserIsAdmin"] = false;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        }
        public ActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();
            return View(lvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                UserViewModel uvm = _userService.GetUsersByEmailAndPassword(lvm.Email, lvm.Password);
                if(uvm != null)
                {
                    Session["CurrentUserId"]=uvm.UserId;
                    Session["CurrentUserName"] = uvm.Name;
                    Session["CurrentUserIsAdmin"]= uvm.IsAdmin;

                    if (uvm.IsAdmin)
                    {
                        return RedirectToRoute(new {area="admin",controller="AdminHome",action="index"});
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("x", "Invalid Email/Password");
                    return View(lvm);
                }
            }
            else
            {
                ModelState.AddModelError("x","Invalid Data");
                return View(lvm);
            }
        }
        [UserAuthorizationFilter]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        [UserAuthorizationFilter]
        public ActionResult EditProfile()
        {
            int uId = Convert.ToInt32(Session["CurrentUserId"]);
            UserViewModel uvm = _userService.GetUserByUserId(uId);
            EditUserDetailsViewModel eudvm = new EditUserDetailsViewModel(){ Email=uvm.Email,Name=uvm.Name,Mobile=uvm.Mobile,UserId=uvm.UserId};
            return View(eudvm);
        }
        [HttpPost]
        [UserAuthorizationFilter]
        public ActionResult EditProfile(EditUserDetailsViewModel euvm)
        {
            if (ModelState.IsValid)
            {
                euvm.UserId = Convert.ToInt32(Session["CurrentUserId"]);
                _userService.UpdateUserDetails(euvm);
                Session["CurrentUserName"] = euvm.Name;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(euvm);
            }
        }
        [UserAuthorizationFilter]
        public ActionResult ChangePassword()
        {
            int uId = Convert.ToInt32(Session["CurrentUserId"]);
            UserViewModel uvm = _userService.GetUserByUserId(uId);
            EditUserPasswordViewModel eudvm = new EditUserPasswordViewModel() { Email = uvm.Email, Password = "", ConfirmPassword = "", UserId = uvm.UserId };
            return View(eudvm);
        }
        [HttpPost]
        [UserAuthorizationFilter]
        public ActionResult ChangePassword(EditUserPasswordViewModel eupvm)
        {
            if (ModelState.IsValid)
            {
                eupvm.UserId = Convert.ToInt32(Session["CurrentUserId"]);
                _userService.UpdateUserPassword(eupvm);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(eupvm);
            }
        }
    }
}