using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WarehouseFrontEnd.Controllers {
    public class InboundController : Controller {
        private readonly string urlOrder = "https://localhost:5100/api/Orders";
        private readonly string urlCustomer = "https://localhost:5100/api/Customers";
        private readonly string urlSupplier = "https://localhost:5100/api/Suppliers";
        private readonly string urlUser = "https://localhost:5100/api/Users";
        private readonly string urlOD = "https://localhost:5100/api/OrderDetails";

        public async Task<IActionResult> Index(string? search) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            List<Order> list = new List<Order>();
            if (search != null) {
                list = await LoadDataAsync<Order>($"{urlOrder}/Inbound?search={search}");
            } else {
                list = await LoadDataAsync<Order>($"{urlOrder}/Inbound");
            }
            var orderDetails = await LoadDataAsync<OrderDetail>($"{urlOD}");

            ViewBag.Inbound = list
                .Select(i => new {
                    i.OrderId,
                    i.CreatedAt,
                    i.OrderType,
                    i.Status,
                    i.Supplier,
                    i.Customer,
                    i.Code,
                    i.User,
                    ProductCount = orderDetails
                                        .Where(od => od.OrderId == i.OrderId)
                                        .Select(od => od.ProductId)
                                        .Distinct()
                                        .Count(),
                    TotalCost = orderDetails
                                        .Where(od => od.OrderId == i.OrderId)
                                        .Sum(od => od.TotalPrice)
                }).ToList();

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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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

        //private string getSearch(string? search) {
        //    if (search == null) {
        //        return "";
        //    }
        //    var firstChar = search.Substring(0, 2);
        //    var lastChar = search.Substring(2);

        //    if (firstChar != "IG") {
        //        return "WTEIYADABFI";
        //    }

        //    var value = int.Parse(lastChar);
        //    return value.ToString();
        //}

    }
}
