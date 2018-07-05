using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCalcForWeb.Models;

namespace SimpleCalcForWeb.Controllers
{
    public class HomeController : Controller
    {
        private static CalcDbContext _db;

        public HomeController(CalcDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index(FormData data)
        {
            if (string.IsNullOrEmpty(data.Expression) == false)
            {
                var note = new Note();
                note.Expression = data.Expression;
                note.Id = Guid.NewGuid();

                int codeError;
                var calc = new Calculator();
                note.Result = calc.Evaluate(note.Expression, out codeError);
                note.CodeError = codeError;

                if (note.Result == null)
                    ViewData["Result"] = "Неудалось подсчтитать ответ.";
                else
                    ViewData["Result"] = "Ответ равен: " + note.Result.ToString();

                note.Date = DateTime.Now;
                note.Host = Request.Host.ToString();

                _db.Notes.Add(note);
                _db.SaveChanges();
            }

            var notes = _db.Notes.OrderByDescending(c => c.Date).AsQueryable();

            if (string.IsNullOrEmpty(data.ExpressionFilter) == false)
                notes = notes.Where(c => c.Expression.Contains(data.ExpressionFilter));

            if (string.IsNullOrEmpty(data.HostFilter) == false)
                notes = notes.Where(c => c.Host.Contains(data.HostFilter));

            ViewBag.History = notes.ToList();
            return View();
        }
    }
}
