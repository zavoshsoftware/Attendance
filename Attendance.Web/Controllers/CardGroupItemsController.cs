using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Attendance.Core.Code;
using Attendance.Models;
using Attendance.Models.Entities;
using Attendance.Web.Services.Errors;

namespace Attendance.Web.Controllers
{
    public class CardGroupItemsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private IErrorService _errors;
        public CardGroupItemsController()
        {
            _errors = new ErrorService();
        }
        // GET: CardGroupItems
        public ActionResult Index(Guid? id)
        {
            ViewBag.GroupId = id;
            var groupItems = db.GroupItems.Where(g=>g.GroupId ==id).Include(c => c.Group).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(groupItems.ToList());
        }

        // GET: CardGroupItems/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroupItem cardGroupItem = db.GroupItems.Find(id);
            if (cardGroupItem == null)
            {
                return HttpNotFound();
            }
            return View(cardGroupItem);
        }


        public ActionResult SubGroups(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cardGroupItems = db.GroupItems.Where(x=>x.ParentGroupId == id.Value);
            var group = db.GroupItems.Find(id);
            ViewBag.GroupId = id;
            return View(cardGroupItems);
        }

        public ActionResult AddSubGroup(Guid parentId)
        {
            var group = db.GroupItems.FirstOrDefault(x => x.Id == parentId);

            ViewBag.ParentGroupId = new SelectList(db.GroupItems.Where(g=>g.Id == group.GroupId),
                "Id", "Title", group);
            ViewBag.IsParent = true;
            return View();
        }

        // GET: CardGroupItems/Create
        public ActionResult Create(Guid groupId)
        {
            var group = db.Groups.FirstOrDefault(x=>x.Id == groupId);
            
                ViewBag.GroupId = new SelectList(db.Groups, "Id", "Title", group);
                ViewBag.IsParent = true;
                return View(); 
        }



        // POST: CardGroupItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CardGroupItem cardGroupItem)
        {
            if (ModelState.IsValid)
            {
				cardGroupItem.IsDeleted=false;
				cardGroupItem.CreationDate= DateTime.Now; 
					
                cardGroupItem.Id = Guid.NewGuid();
                db.GroupItems.Add(cardGroupItem);
                db.SaveChanges();
                return RedirectToAction("Index",new { id = cardGroupItem.GroupId});
            }

            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Title", cardGroupItem.GroupId);
            return View(cardGroupItem);
        }

        // GET: CardGroupItems/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroupItem cardGroupItem = db.GroupItems.Find(id);
            if (cardGroupItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Title", cardGroupItem.GroupId);

            return View(cardGroupItem);
        }

        // POST: CardGroupItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,GroupId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardGroupItem cardGroupItem)
        {
            if (ModelState.IsValid)
            {
				cardGroupItem.IsDeleted=false;
					cardGroupItem.LastModifiedDate=DateTime.Now;
                db.Entry(cardGroupItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = cardGroupItem.GroupId });
            }
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Title", cardGroupItem.GroupId);
            return View(cardGroupItem);
        }

        // GET: CardGroupItems/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroupItem cardGroupItem = db.GroupItems.Find(id);
            if (cardGroupItem == null)
            {
                return HttpNotFound();
            }
            return View(cardGroupItem);
        }

        // POST: CardGroupItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CardGroupItem cardGroupItem = db.GroupItems.Find(id);
			cardGroupItem.IsDeleted=true;
			cardGroupItem.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = cardGroupItem.GroupId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GroupItemCards(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroupItem cardGroupItem = db.GroupItems.Find(id);
            if (cardGroupItem == null)
            {
                return HttpNotFound();
            }
            var cards = cardGroupItem.GroupItemCards.Select(c => c.Card).Distinct().ToList();
            ViewBag.cardGroupTitle = cardGroupItem.Title;
            ViewBag.cardGroupItemId = cardGroupItem.Id;
            return View(cards);
        }

        public ActionResult CardStatus(Guid itemId,bool status)
        {
            var id = itemId;
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroupItem cardGroupItem = db.GroupItems.Find(id);
            if (cardGroupItem == null)
            {
                return HttpNotFound();
            }
            try
            {
                var statusStr = status ? "فعال" : "غیرفعال ";
                TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                cardGroupItem.GroupItemCards.Select(c => c.Card).Distinct().ToList().ForEach(c => {
                    c.IsActive = status;
                    db.Entry(c).State = EntityState.Modified;
                    db.CardStatusHistories.Add(new CardStatusHistory()
                    {
                        Card =c,
                        CardId = c.Id,
                        Id=Guid.NewGuid(),
                        CreationDate=DateTime.Now,
                        IsActive = true,
                        Description=$"بصورت گروهی {statusStr} شد",
                        PreviousStatus = c.IsActive,
                        CurrentStatus = status
                    });
                });
                db.SaveChanges();
            }
            catch (Exception e)
            {
                string errors = _errors.HandleError(e, Debugger.Info());
                TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = $"خطایی پیش آمده{errors}" };
            }
            return RedirectToAction("GroupItemCards", new { id = id});
        }

    }
}
