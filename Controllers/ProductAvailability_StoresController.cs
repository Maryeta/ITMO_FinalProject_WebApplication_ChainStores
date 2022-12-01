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
    public class ProductAvailability_StoresController : Controller
    {
        private ChainStoresEntities db = new ChainStoresEntities();

        // GET: ProductAvailability_Stores
        public ActionResult Index()
        {
            return View(db.Stores.ToList());
        }

        public ActionResult AddNewProduct(int? id)
        {
            ViewBag.StoreId = id;
            //return View(db.Product.ToList());
            
            var query = db.productNotStore(id);
            return View(query);
        }

        public ActionResult StoreProc()
        {
            DateTime date = new DateTime(2022, 9, 08);
            var proc = db.orderChainStores(date);
            return View(proc);
        }
        //GET
        public ActionResult AddNewProductSelect(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ProductID = id1;
            ViewBag.StoreID = id2;
            ProductAvailability_Stores newStoreProd = new ProductAvailability_Stores();
            newStoreProd.ProductID = (int)id1;
            newStoreProd.StoreID = (int)id2;
            db.ProductAvailability_Stores.Add(newStoreProd);
            db.SaveChanges();
            return View(newStoreProd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewProductSelect([Bind(Include = "ProductID, StoreID, Quantity")] ProductAvailability_Stores productAvailability_Stores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productAvailability_Stores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddNewProduct", new { id = productAvailability_Stores.StoreID });
            }
            return View(productAvailability_Stores);
        }

        // GET: ProductAvailability_Stores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAvailability_Stores productAvailability_Stores = db.ProductAvailability_Stores.Find(id);
            if (productAvailability_Stores == null)
            {
                return HttpNotFound();
            }
            return View(productAvailability_Stores);
        }

        // GET: ProductAvailability_Stores/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName");
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City");
            return View();
        }

        // POST: ProductAvailability_Stores/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,StoreID,Quantity")] ProductAvailability_Stores productAvailability_Stores)
        {
            if (ModelState.IsValid)
            {
                db.ProductAvailability_Stores.Add(productAvailability_Stores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", productAvailability_Stores.ProductID);
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City", productAvailability_Stores.StoreID);
            return View(productAvailability_Stores);
        }

        // GET: ProductAvailability_Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAvailability_Stores productAvailability_Stores = db.ProductAvailability_Stores.Find(id);
            if (productAvailability_Stores == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", productAvailability_Stores.ProductID);
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City", productAvailability_Stores.StoreID);
            return View(productAvailability_Stores);
        }

        // POST: ProductAvailability_Stores/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,StoreID,Quantity")] ProductAvailability_Stores productAvailability_Stores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productAvailability_Stores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", productAvailability_Stores.ProductID);
            ViewBag.StoreID = new SelectList(db.Stores, "StoreID", "City", productAvailability_Stores.StoreID);
            return View(productAvailability_Stores);
        }

        // GET: ProductAvailability_Stores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAvailability_Stores productAvailability_Stores = db.ProductAvailability_Stores.Find(id);
            if (productAvailability_Stores == null)
            {
                return HttpNotFound();
            }
            return View(productAvailability_Stores);
        }

        // POST: ProductAvailability_Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductAvailability_Stores productAvailability_Stores = db.ProductAvailability_Stores.Find(id);
            db.ProductAvailability_Stores.Remove(productAvailability_Stores);
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
