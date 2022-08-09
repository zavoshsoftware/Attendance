//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Attendance.Models;
//using Attendance.Models.Entities;

//namespace Attendance.Web.Controllers
//{
//    public class CardDaysController : Controller
//    {
//        private DatabaseContext db = new DatabaseContext();

//        // GET: CardDays
//        public ActionResult Index(Guid id)
//        {
//            var card = db.Cards.Find(id);

//            if (card != null)
//                ViewBag.Title = "فهرست روزهای مجاز کارت کد " + card.Code;

//            var cardDays = db.CardDays.Include(c => c.Card).Where(c=>c.CardId==id&& c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
//            return View(cardDays.ToList());
//        }
         
//        public ActionResult Create(Guid id)
//        {
//            ViewBag.CardId = id;
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(CardDay cardDay, Guid id)
//        {
//            if (ModelState.IsValid)
//            {
//				cardDay.IsDeleted=false;
//				cardDay.CreationDate= DateTime.Now;
//                cardDay.CardId = id;
//                cardDay.Id = Guid.NewGuid();
//                db.CardDays.Add(cardDay);
//                db.SaveChanges();
//                return RedirectToAction("Index",new {id=id});
//            }

//            ViewBag.CardId = id;
//            return View(cardDay);
//        }

//        // GET: CardDays/Edit/5
//        public ActionResult Edit(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CardDay cardDay = db.CardDays.Find(id);
//            if (cardDay == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.CardId = cardDay.CardId;
//            return View(cardDay);
//        }

 
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(CardDay cardDay)
//        {
//            if (ModelState.IsValid)
//            {
//				cardDay.IsDeleted=false;
//					cardDay.LastModifiedDate=DateTime.Now;
//                db.Entry(cardDay).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index", new { id = cardDay.CardId });
//            }
//            ViewBag.CardId =  cardDay.CardId;
//            return View(cardDay);
//        }

//        // GET: CardDays/Delete/5
//        public ActionResult Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CardDay cardDay = db.CardDays.Find(id);
//            if (cardDay == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.CardId =  cardDay.CardId;
//            return View(cardDay);
//        }

//        // POST: CardDays/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(Guid id)
//        {
//            CardDay cardDay = db.CardDays.Find(id);
//			cardDay.IsDeleted=true;
//			cardDay.DeletionDate=DateTime.Now;
 
//            db.SaveChanges();
//            return RedirectToAction("Index", new { id = cardDay.CardId });
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
