using System;
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
        private bool HistoryIsRead { get; set; }

        [HttpGet]
        public IActionResult Index(string src = null)
        {
            var historyNotes = History.GetHistory();

            if (src != null && src.Length > 0)
            {
                int codeError;
                Calculator calc = new Calculator();
                var result = calc.Evaluate(src, out codeError).ToString();

                switch (codeError)
                {
                    case 0:
                        result = $"Результат вычислений равен: {result}";
                        break;
                    case 1:
                        result = "Вы ввели неизвестную операцию.";
                        break;
                    case 2:
                        result = "Неверный формат строки.";
                        break;
                    case 3:
                        result = "Неверное соотношение цифр и арифметических операций.";
                        break;
                    default:
                        result = $"Неизвестная ошибка: {src}";
                        break;
                }

                ViewData["Result"] = result;

                History.PutRecord(result);
                historyNotes.Insert(0, result);
            }

            ViewBag.Array = historyNotes;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Page about SimpleCalc.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "My contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
