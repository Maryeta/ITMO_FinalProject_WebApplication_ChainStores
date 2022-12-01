using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication_ChainStores.Models;

namespace WebApplication_ChainStores.Controllers
{
    public class Orders_StoresController : Controller
    {
        private ChainStoresEntities db = new ChainStoresEntities();

        // GET: Orders_Stores
        public ActionResult Index()
        {
            var orders_Stores = db.Orders_Stores.Include(o => o.Employees).Include(o => o.Stores);
            return View(orders_Stores.ToList());
        }

        public ActionResult ListStore()
        {
            return View(db.Stores.ToList());
        }


        public ActionResult AddNewOrder(int? id)
        {
            string login = "";
            int? employeeID = null;
            if (User.Identity.IsAuthenticated)
            {
                login = User.Identity.Name;
            }
            var search = from reg in db.Registration
                         where reg.Login == login
                         select reg;
            foreach (var emp in search)
            {
                employeeID = emp.EmployeeID;
            }
            ViewBag.StoreID = id;
            Orders_Stores orderStore = new Orders_Stores();
            orderStore.StoreID = (int)id;
            orderStore.Date = DateTime.Today;
            orderStore.EmployeeID = (int)employeeID;
            db.Orders_Stores.Add(orderStore);
            db.SaveChanges();

            Employees empOrd = db.Employees.Find(employeeID);

            ViewBag.EmployeeLastName = empOrd.LastName;
            return View(orderStore);
        }

        public ActionResult AddProductInOrder(int? id)
        {
            ViewBag.OrderStoreID = id;
            return View(db.Product.ToList());
        }

        public ActionResult AddThisProduct(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Details_Store orderDetails = new Orders_Details_Store();
            orderDetails.ProductID = (int)id1;
            orderDetails.OrderStoreID = (int)id2;
            db.Orders_Details_Store.Add(orderDetails);
            db.SaveChanges();
            return View(orderDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddThisProduct([Bind(Include = "ProductID, OrderStoreID, Quantity")] Orders_Details_Store orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                db.SaveChanges();
                int idstore = orderDetails.OrderStoreID;
                Orders_Stores orderStore = db.Orders_Stores.Find(idstore);
                return RedirectToAction("AddProductInOrder", new { id = orderStore.OrderStoreID});
            }
            return View(orderDetails);
        }

        public ActionResult ActualOrderStore (int? id)
        {
            Orders_Stores orderStore = db.Orders_Stores.Find(id);
            return View(orderStore);
        }
        public ActionResult ResultOrder(int? id)
        {
            Orders_Stores orderStore = db.Orders_Stores.Find(id);
            return View(orderStore);
        }

        public ActionResult ListStoreReady()
        {
            return View(db.Stores.ToList());
        }

        public ActionResult ReadyOrdersStore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var storeOrders = from ord in db.Orders_Stores
                              where ord.StoreID == id
                              select ord;
            return View(storeOrders);
        }

        public ActionResult DetailsOrder (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Stores orders_Stores = db.Orders_Stores.Find(id);
            if (orders_Stores == null)
            {
                return HttpNotFound();
            }
            return View(orders_Stores);
        }


        // GET: Orders_Stores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Stores orders_Stores = db.Orders_Stores.Find(id);
            if (orders_Stores == null)
            {
                return HttpNotFound();
            }
            return View(orders_Stores);
        }

        // GET: Orders_Stores/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post");
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City");
            return View();
        }

        // POST: Orders_Stores/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderStoreID,StoreID,Date,EmployeeID")] Orders_Stores orders_Stores)
        {
            if (ModelState.IsValid)
            {
                db.Orders_Stores.Add(orders_Stores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post", orders_Stores.EmployeeID);
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City", orders_Stores.StoreID);
            return View(orders_Stores);
        }

        // GET: Orders_Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Stores orders_Stores = db.Orders_Stores.Find(id);
            if (orders_Stores == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post", orders_Stores.EmployeeID);
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City", orders_Stores.StoreID);
            return View(orders_Stores);
        }

        // POST: Orders_Stores/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderStoreID,StoreID,Date,EmployeeID")] Orders_Stores orders_Stores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders_Stores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post", orders_Stores.EmployeeID);
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City", orders_Stores.StoreID);
            return View(orders_Stores);
        }

        // GET: Orders_Stores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Stores orders_Stores = db.Orders_Stores.Find(id);
            if (orders_Stores == null)
            {
                return HttpNotFound();
            }
            return View(orders_Stores);
        }

        // POST: Orders_Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders_Stores orders_Stores = db.Orders_Stores.Find(id);
            db.Orders_Stores.Remove(orders_Stores);
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
