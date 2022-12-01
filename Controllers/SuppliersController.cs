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
    public class SuppliersController : Controller
    {
        private ChainStoresEntities db = new ChainStoresEntities();

        // GET: Suppliers
        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        public ActionResult ProductSupplier(int? id)
        {
            ViewBag.SupplierID = id;
            var query = from sap in db.ProductAvailability_Suppliers
                        where sap.SupplierID == id
                        select sap;
            Suppliers supplier = db.Suppliers.Find(id);
            ViewBag.SupplierName = supplier.SupplierName;
            return View(query);
        }

        public ActionResult AddProductSupplier(int? id)
        {
            ViewBag.SupplierID = id;
            var query = db.productNotSupplier(id);
            Suppliers supplier = db.Suppliers.Find(id);
            ViewBag.SupplierName = supplier.SupplierName;
            return View(query);
        }

        public ActionResult AddNewProductSelectSupplier(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ProductID = id1;
            ViewBag.StoreID = id2;
            ProductAvailability_Suppliers newSupProd = new ProductAvailability_Suppliers();
            newSupProd.ProductID = (int)id1;
            newSupProd.SupplierID = (int)id2;
            db.ProductAvailability_Suppliers.Add(newSupProd);
            db.SaveChanges();
            return View(newSupProd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewProductSelectSupplier ([Bind(Include = "ProductID, SupplierID, Quantity")] ProductAvailability_Suppliers prodSup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prodSup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddProductSupplier", new { id = prodSup.SupplierID });
            }
            return View(prodSup);
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierID,SupplierName,Country,City,Street,Building,PhoneNumber,Email")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(suppliers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(suppliers);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Suppliers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,SupplierName,Country,City,Street,Building,PhoneNumber,Email")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suppliers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = db.Suppliers.Find(id);
            db.Suppliers.Remove(suppliers);
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
