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
    public class Orders_Details_SuppliersController : Controller
    {
        private ChainStoresEntities db = new ChainStoresEntities();

        // GET: Orders_Details_Suppliers
        public ActionResult Index()
        {
            var orders_Details_Suppliers = db.Orders_Details_Suppliers.Include(o => o.Orders_Suppliers).Include(o => o.Product);
            return View(orders_Details_Suppliers.ToList());
        }

        // GET: Orders_Details_Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Details_Suppliers orders_Details_Suppliers = db.Orders_Details_Suppliers.Find(id);
            if (orders_Details_Suppliers == null)
            {
                return HttpNotFound();
            }
            return View(orders_Details_Suppliers);
        }

        // GET: Orders_Details_Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.OrderSupplierID = new SelectList(db.Orders_Suppliers, "OrderSupplierID", "OrderSupplierID");
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName");
            return View();
        }

        // POST: Orders_Details_Suppliers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderSupplierID,ProductID,Quantity")] Orders_Details_Suppliers orders_Details_Suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Orders_Details_Suppliers.Add(orders_Details_Suppliers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderSupplierID = new SelectList(db.Orders_Suppliers, "OrderSupplierID", "OrderSupplierID", orders_Details_Suppliers.OrderSupplierID);
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", orders_Details_Suppliers.ProductID);
            return View(orders_Details_Suppliers);
        }

        // GET: Orders_Details_Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Details_Suppliers orders_Details_Suppliers = db.Orders_Details_Suppliers.Find(id);
            if (orders_Details_Suppliers == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderSupplierID = new SelectList(db.Orders_Suppliers, "OrderSupplierID", "OrderSupplierID", orders_Details_Suppliers.OrderSupplierID);
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", orders_Details_Suppliers.ProductID);
            return View(orders_Details_Suppliers);
        }

        // POST: Orders_Details_Suppliers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderSupplierID,ProductID,Quantity")] Orders_Details_Suppliers orders_Details_Suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders_Details_Suppliers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderSupplierID = new SelectList(db.Orders_Suppliers, "OrderSupplierID", "OrderSupplierID", orders_Details_Suppliers.OrderSupplierID);
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", orders_Details_Suppliers.ProductID);
            return View(orders_Details_Suppliers);
        }

        // GET: Orders_Details_Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders_Details_Suppliers orders_Details_Suppliers = db.Orders_Details_Suppliers.Find(id);
            if (orders_Details_Suppliers == null)
            {
                return HttpNotFound();
            }
            return View(orders_Details_Suppliers);
        }

        // POST: Orders_Details_Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders_Details_Suppliers orders_Details_Suppliers = db.Orders_Details_Suppliers.Find(id);
            db.Orders_Details_Suppliers.Remove(orders_Details_Suppliers);
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
