using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalcForWeb.Models;

namespace SimpleCalcForWeb.Controllers
{
    public class HomeController : Controller
    {
        private static Repository _db;

        public HomeController(Repository context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            ViewBag.History = _db.Notes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(Note note)
        {
            if (note.Expression != null && note.Expression.Length > 0)
            {
                int codeError;
                Calculator calc = new Calculator();
                note.Result = calc.Evaluate(note.Expression, out codeError).ToString();

                switch (codeError)
                {
                    case 0: 
                        break;
                    case 1:
                        note.Result = "Вы ввели неизвестную операцию.";
                        break;
                    case 2:
                        note.Result = "Неверный формат строки.";
                        break;
                    case 3:
                        note.Result = "Неверное соотношение цифр и арифметических операций.";
                        break;
                    default:
                        note.Result = $"Неизвестная ошибка: {note.Expression}";
                        break;
                }

                ViewData["Result"] = "Ответ равен: " + note.Result;

                _db.Notes.Add(note);
                _db.SaveChanges();
            }

            ViewBag.History = _db.Notes.ToList();
            return View();
        }
    }
}
