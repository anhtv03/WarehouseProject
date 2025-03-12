using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;

namespace WarehouseFrontEnd.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.UserRole = current_user;
            }


            return View(current_user);
        }


    }
}
