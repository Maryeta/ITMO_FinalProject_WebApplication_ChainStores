using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication_ChainStores.Models;

namespace WebApplication_ChainStores.Controllers
{
    public class Orders_SuppliersController : Controller
    {
        private ChainStoresEntities db = new ChainStoresEntities();

        // GET: Orders_Suppliers
        public ActionResult Index()
        {
            var orders_Suppliers = db.Orders_Suppliers.Include(o => o.Employees).Include(o => o.Suppliers);
            return View(orders_Suppliers.ToList());
        }

        public ActionResult ListSupplier()
        {
            return View(db.Suppliers.ToList());
        }

        public ActionResult NewOrderSupplier(int? id)
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
            Orders_Suppliers orderSupplier = new Orders_Suppliers();
            orderSupplier.SupplierID = (int)id;
            orderSupplier.Date = DateTime.Today;
            orderSupplier.EmployeeID = (int)employeeID;
            db.Orders_Suppliers.Add(orderSupplier);
            db.SaveChanges();

            Employees empOrd = db.Employees.Find(employeeID);
            Suppliers supplier = db.Suppliers.Find(id);
            ViewBag.EmployeeLastName = empOrd.LastName;
            ViewBag.SupplierID = supplier.SupplierID;
            return View(orderSupplier);
        }

        public ActionResult AddProductInOrderSupplier(int? id1, int? id2)
        {
            var query = db.productSupplier(id2);
            ViewBag.OrderSupplierID = id1;
            ViewBag.SupplierID = id2;
            return View(query);
        }

        public ActionResult AllOrdersSuppliers()
        {
            return View(db.Orders_Suppliers.ToList());
        }

        public ActionResult AddSelectProductOrdSup(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Details_Suppliers orderDetailsSup = new Orders_Details_Suppliers();
            orderDetailsSup.ProductID = (int)id1;
            orderDetailsSup.OrderSupplierID = (int)id2;
            db.Orders_Details_Suppliers.Add(orderDetailsSup);
            db.SaveChanges();
            return View(orderDetailsSup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSelectProductOrdSup([Bind(Include = "OrderSupplierID, ProductID, Quantity")] Orders_Details_Suppliers orderDetSup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetSup).State = EntityState.Modified;
                db.SaveChanges();
                int idsup = orderDetSup.OrderSupplierID;
                Orders_Suppliers orderSup = db.Orders_Suppliers.Find(idsup);
                return RedirectToAction("AddProductInOrderSupplier", new { id1 = orderSup.OrderSupplierID, id2 = orderSup.SupplierID });
            }
            return View(orderDetSup);
        }

        public ActionResult ActualOrderSupplier (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Suppliers orderSup = db.Orders_Suppliers.Find(id);
            ViewBag.OrderSupID = id;
            return View(orderSup);
        }

        public ActionResult DetailOrderSupplier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Suppliers orderSup = db.Orders_Suppliers.Find(id);
            return View(orderSup);
        }

        // GET: Orders_Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Suppliers orders_Suppliers = db.Orders_Suppliers.Find(id);
            if (orders_Suppliers == null)
            {
                return HttpNotFound();
            }
            return View(orders_Suppliers);
        }

        public ActionResult SumOrderStores()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SumOrderStores(DateTime date)
        {
            return RedirectToAction("SumOrderStoresView", new {d = date });
        }

        public ActionResult SumOrderStoresView(DateTime d)
        {
            var proc = db.orderChainStores(d);
            return View(proc);
        }

        public ActionResult SupplierProduct(int? id)
        {
            Product query = db.Product.Find(id);
            return View(query);
        }

        public ActionResult EditDetails(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Details_Suppliers orders_Details_Suppliers = db.Orders_Details_Suppliers.Find(id1, id2);
            if (orders_Details_Suppliers == null)
            {
                return HttpNotFound();
            }
            return View(orders_Details_Suppliers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails([Bind(Include = "OrderSupplierID,ProductID,Quantity")] Orders_Details_Suppliers orders_Details_Suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders_Details_Suppliers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ActualOrderSupplier", new { id = orders_Details_Suppliers.OrderSupplierID });
            }
          
            return View(orders_Details_Suppliers);
        }

        // GET: Orders_Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName");
            return View();
        }

        // POST: Orders_Suppliers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderSupplierID,SupplierID,Date,EmployeeID")] Orders_Suppliers orders_Suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Orders_Suppliers.Add(orders_Suppliers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post", orders_Suppliers.EmployeeID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", orders_Suppliers.SupplierID);
            return View(orders_Suppliers);
        }

        // GET: Orders_Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Suppliers orders_Suppliers = db.Orders_Suppliers.Find(id);
            if (orders_Suppliers == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post", orders_Suppliers.EmployeeID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", orders_Suppliers.SupplierID);
            return View(orders_Suppliers);
        }

        // POST: Orders_Suppliers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderSupplierID,SupplierID,Date,EmployeeID")] Orders_Suppliers orders_Suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders_Suppliers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Post", orders_Suppliers.EmployeeID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", orders_Suppliers.SupplierID);
            return View(orders_Suppliers);
        }

        // GET: Orders_Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Suppliers orders_Suppliers = db.Orders_Suppliers.Find(id);
            if (orders_Suppliers == null)
            {
                return HttpNotFound();
            }
            return View(orders_Suppliers);
        }

        // POST: Orders_Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders_Suppliers orders_Suppliers = db.Orders_Suppliers.Find(id);
            db.Orders_Suppliers.Remove(orders_Suppliers);
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
