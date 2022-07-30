using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Attendance.Models;
using Attendance.Models.Entities;

namespace Attendance.Web.Controllers.api
{
    public class CardsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Cards
        public IQueryable<Card> GetCards()
        {
            return db.Cards;
        }

        // GET: api/Cards/5
        [ResponseType(typeof(Card))]
        public IHttpActionResult GetCard(Guid id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            return Ok(card);
        }

        // PUT: api/Cards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCard(Guid id, Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.Id)
            {
                return BadRequest();
            }

            db.Entry(card).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cards
        [ResponseType(typeof(Card))]
        public IHttpActionResult PostCard(Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cards.Add(card);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CardExists(card.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = card.Id }, card);
        }

        // DELETE: api/Cards/5
        [ResponseType(typeof(Card))]
        public IHttpActionResult DeleteCard(Guid id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(Guid id)
        {
            return db.Cards.Count(e => e.Id == id) > 0;
        }
    }
}