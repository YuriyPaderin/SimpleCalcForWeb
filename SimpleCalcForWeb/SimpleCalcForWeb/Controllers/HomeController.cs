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

        [HttpGet]
        public IActionResult Index(FormData data)
        {
            if (string.IsNullOrEmpty(data.Expression) == false && data.TypeButton == "Расчитать")
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

                note.DateAndTime = DateTime.Now;
                note.HostName = Request.Host.ToString();

                _db.Notes.Add(note);
                _db.SaveChanges();
            }

            var notes = _db.Notes.OrderByDescending(c => c.DateAndTime).AsQueryable();

            if (string.IsNullOrEmpty(data.ExpressionFilter) == false)
                notes = notes.Where(note => EF.Functions.Like(note.Expression, "%" + data.ExpressionFilter + "%"));

            if (string.IsNullOrEmpty(data.HostFilter) == false)
                notes = notes.Where(note => EF.Functions.Like(note.HostName, "%" + data.HostFilter + "%"));

            var history = new List<History>();
            foreach(var note in notes.ToList())
            {
                string message;
                switch (note.ErrorCode)
                {
                    case 0:
                        message = note.Result.ToString();
                        break;
                    case 1:
                        message = "Вы ввели неизвестную операцию.";
                        break;
                    case 2:
                        message = "Неверный формат строки.";
                        break;
                    case 3:
                        message = "Неверное соотношение цифр и арифметических операций.";
                        break;
                    default:
                        message = "Неизвестная ошибка.";
                        break;
                }

                history.Add(new History(note.Expression, message, note.HostName));
            }

            ViewBag.History = history;
            

            return View();
        }
    }
}
