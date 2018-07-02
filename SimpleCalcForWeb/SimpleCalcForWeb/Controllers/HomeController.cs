using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleCalcForWeb.Models;
using System.Collections.Generic;

namespace SimpleCalcForWeb.Controllers
{
    public class HomeController : Controller
    {
        private static CalcDbContext _db;

        public HomeController(CalcDbContext context)
        {
            _db = context;
        }

        public IActionResult Index(Filter filter)
        {
            IQueryable<Note> notes = _db.Notes;

            if (filter.Expression != null && filter.Expression.Length > 0)
                notes = notes.Where(c => c.Expression.Contains(filter.Expression));

            if (filter.Host != null && filter.Host.Length > 0)
                notes = notes.Where(c => c.Host.Contains(filter.Host));

            ViewBag.History = notes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(Note note)
        {
            if (note.Expression != null && note.Expression.Length > 0)
            {
                note.Id = Guid.NewGuid().ToString();

                int codeError = 0;
                Calculator calc = new Calculator();
                note.Result = calc.Evaluate(note.Expression, out codeError);
                note.CodeError = codeError;
                if (note.CodeError > 0)
                    note.Result = null;

                note.Date = DateTime.Now;
                note.Host = Request.Host.ToString();

                ViewData["Result"] = "Ответ равен: " + note.Result.ToString();

                _db.Notes.Add(note);
                _db.SaveChanges();
            }

            ViewBag.History = _db.Notes.ToList();
            return View();
        }
    }
}
