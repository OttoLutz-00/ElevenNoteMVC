using ElevenNote.Models.NoteModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    public class NoteController : Controller
    {
        // access to service layer
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }

        // GET: Note/Index
        [Authorize]
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            var model = service.GetNotes();
            return View(model);
        }

        // GET: Note/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Note/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateNoteService();

            if (!service.CreateNote(model))
            {
                ModelState.AddModelError("", "Note could not be created.");
                return View(model);
            }
            TempData["SaveResult"] = "Your Note was created.";

            return RedirectToAction("Index");
        }

        // GET: Note/Details/{id}
        public ActionResult Details(int id)
        {
            var service = CreateNoteService();
            var note = service.GetNoteById(id);

            return View(note);
        }

        // GET: Note/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var note = service.GetNoteById(id);
            var model = new NoteEdit()
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Content = note.Content
            };

            return View(model);
        }

        // POST: Note/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }

        // GET: Note/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);
            return View(model);
        }

        // POST: Note/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();
            service.DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted.";

            return RedirectToAction("Index");
        }

    }
}