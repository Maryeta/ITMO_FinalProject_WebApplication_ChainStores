using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication_ChainStores.Models;

namespace WebApplication_ChainStores.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Employees emp = null;
                using (ChainStoresEntities db = new ChainStoresEntities())
                {
                    emp = db.Employees.FirstOrDefault(e => e.Registration.Login == model.Login && e.Registration.Password == model.Password);
                }
                if (emp != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Products");

                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }

            }
            return View(model);
        }

        public ActionResult AccountEmployee()
        {
            Employees emp = null;
            using (ChainStoresEntities db = new ChainStoresEntities())
            {
                emp = db.Employees.FirstOrDefault(e => e.Registration.Login == User.Identity.Name);
            }
            return View(emp);
        }

        public ActionResult Out()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}