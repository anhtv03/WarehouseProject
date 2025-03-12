using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WarehouseFrontEnd.Controllers {
    public class InboundController : Controller {
        private readonly string urlOrder = "https://localhost:5100/api/Orders";
        private readonly string urlCategory = "https://localhost:5100/api/Categories";
        private readonly string urlSupplier = "https://localhost:5100/api/Suppliers";

        public async Task<IActionResult> IndexAsync(string? search) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }
            List<Order> list = new List<Order>();
            if (search != null) {
                list = await LoadDataAsync<Order>($"{urlOrder}?search={search}");
            } else {
                list = await LoadDataAsync<Order>(urlOrder);
            }

            ViewBag.Inbound = list;
            ViewBag.SearchValue = search ?? "";
            return View();
        }

        // GET: InboundController/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // GET: InboundController/Create
        public ActionResult Create() {
            return View();
        }

        // POST: InboundController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) {
            try {
                return RedirectToAction(nameof(IndexAsync));
            } catch {
                return View();
            }
        }

        // GET: InboundController/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: InboundController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(IndexAsync));
            } catch {
                return View();
            }
        }

        // GET: InboundController/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: InboundController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(IndexAsync));
            } catch {
                return View();
            }
        }

        //=======================================================================================================     
        private async Task<List<T>> LoadDataAsync<T>(string url) {
            List<T> dataList = new List<T>();
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync(url)) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        dataList = JsonConvert.DeserializeObject<List<T>>(data);
                    }
                }
            }
            return dataList;
        }


    }
}
