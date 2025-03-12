using Microsoft.AspNetCore.Mvc;
using WarehouseFrontEnd.Models.DTOs;


namespace WarehouseFrontEnd.Controllers {
    public class AuthController : Controller {
        private readonly string urlAuth = "https://localhost:5100";

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO user) {
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(urlAuth + "/Login", user)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        HttpContext.Session.SetString("User", data);
                        return RedirectToAction("Index", "Home");
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Index", user);
                    }
                }
            }
        }

        public ActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Auth");
        }

    }
}
