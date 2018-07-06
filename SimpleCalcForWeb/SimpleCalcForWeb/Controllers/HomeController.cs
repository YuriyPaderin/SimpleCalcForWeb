using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCalcForWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleCalcForWeb.Controllers
{
    public class HomeController : Controller
    {
        private static CalcDbContext _db;

        public HomeController(CalcDbContext context)
        {
            _db = context;
        }

        public object SqlMethods { get; private set; }

        [HttpGet]
        public IActionResult Index(FormData data)
        {
            if (string.IsNullOrEmpty(data.Expression) == false && data.TypeButton == "Расчет")
            {
                var note = new Note();
                note.Expression = data.Expression;
                note.Id = Guid.NewGuid();

                int errorCode;
                var calc = new Calculator();
                note.Result = calc.Evaluate(note.Expression, out errorCode);
                note.ErrorCode = errorCode;

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
                notes = from note in notes
                    where EF.Functions.Like(note.Expression, "%" + data.ExpressionFilter + "%")
                    select note;

            if (string.IsNullOrEmpty(data.HostFilter) == false)
                notes = from note in notes
                    where EF.Functions.Like(note.Host, "%" + data.HostFilter + "%")
                    select note;

            ViewBag.History = notes.ToList();
            return View();
        }
    }
}
