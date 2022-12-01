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
    public class StoresController : Controller
    {
        private ChainStoresEntities db = new ChainStoresEntities();

        // GET: Stores
        public ActionResult Index()
        {
            return View(db.Stores.ToList());
        }



        public ActionResult StoreProduct(int? id)
        {
            var countProduct = from pa in db.ProductAvailability_Stores
                       join p in db.Product on pa.ProductID equals p.ProductID
                       where pa.StoreID == id
                       select pa;
            Stores store = db.Stores.Find(id);
            ViewBag.StoreStreet = store.Street;
            return View(countProduct);
        }

        public ActionResult ProductNeed(int? id)
        {
            var Need = from pa in db.ProductAvailability_Stores
                       join p in db.Product on pa.ProductID equals p.ProductID
                       where pa.StoreID == id && pa.Quantity < 5
                       select pa;
            Stores store = db.Stores.Find(id);
            ViewBag.StoreStreet = store.Street;
            return View(Need);
        }

        public ActionResult SearchProductStore(int? id)
        {
            var countProduct = from pa in db.ProductAvailability_Stores
                               join p in db.Product on pa.ProductID equals p.ProductID
                               where pa.StoreID == id
                               select pa;
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "Street");
            return PartialView(countProduct.ToList());
        }

        // GET: Stores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            return View(stores);
        }

        // GET: Stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StoreID,City,Street,Building")] Stores stores)
        {
            if (ModelState.IsValid)
            {
                db.Stores.Add(stores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stores);
        }

        public ActionResult EditQuantity (int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAvailability_Stores stock = db.ProductAvailability_Stores.Find(id1, id2);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuantity([Bind(Include = "ProductID,StoreID,Quantity")]  ProductAvailability_Stores stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("StoreProduct", new { id = stock.StoreID});
            }
            return View(stock);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            return View(stores);
        }

        // POST: Stores/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StoreID,City,Street,Building")] Stores stores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stores);
        }

        // GET: Stores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores stores = db.Stores.Find(id);
            if (stores == null)
            {
                return HttpNotFound();
            }
            return View(stores);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stores stores = db.Stores.Find(id);
            db.Stores.Remove(stores);
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
