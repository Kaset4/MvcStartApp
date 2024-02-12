using Microsoft.AspNetCore.Mvc;
using MvcStartApp.Models.Db;
using MvcStartApp.Models;
using System.Diagnostics;

namespace MvcStartApp.Controllers
{
    public class FeedbackController: Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Feedback feedback)
        {
            return StatusCode(200, $"{feedback.From}, спасибо за отзыв!");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
