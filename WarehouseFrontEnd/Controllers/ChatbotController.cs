using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;

namespace WarehouseFrontEnd.Controllers {
    public class ChatbotController : Controller {
        public IActionResult Index() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            return View();
        }
    }
}
