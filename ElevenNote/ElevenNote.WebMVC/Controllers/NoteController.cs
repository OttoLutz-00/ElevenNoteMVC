using ElevenNote.Models.NoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    public class NoteController : Controller
    {

        // GET: Note/Index
        [Authorize]
        public ActionResult Index()
        {
            var model = new NoteListItem[0];
            return View(model);
        }




        // GET: Note/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
    }
}