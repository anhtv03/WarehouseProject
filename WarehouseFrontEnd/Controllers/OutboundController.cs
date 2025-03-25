using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;

using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace WarehouseFrontEnd.Controllers {
    public class OutboundController : Controller {
        private readonly string urlOrder = "https://localhost:5100/api/Orders";
        private readonly string urlProduct = "https://localhost:5100/api/Products";
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
                list = await LoadDataAsync<Order>($"{urlOrder}/Outbound?search={search}");
            } else {
                list = await LoadDataAsync<Order>($"{urlOrder}/Outbound");
            }
            var orderDetails = await LoadDataAsync<OrderDetail>($"{urlOD}");

            ViewBag.Outbound = list
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

        public async Task<IActionResult> Details(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            Order order = new Order();
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync($"{urlOrder}/{id}")) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        order = JsonConvert.DeserializeObject<Order>(data);
                    }
                }
            }

            ViewBag.totalCost = order?.OrderDetails.Sum(od => od.TotalPrice) ?? 0;
            ViewBag.totalProduct = order?.OrderDetails.Count ?? 0;
            return View(order);
        }

        public async Task<IActionResult> Create() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            var listUser = await LoadDataAsync<User>(urlUser);

            ViewBag.Customer = await LoadDataAsync<Customer>(urlCustomer);
            ViewBag.Users = listUser.Where(x => x.Role != 1).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDTO order, List<OrderDetailDTO> OrderItems) {
            order.OrderType = "Outbound";
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(urlOrder, order)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        OrderItems.ForEach(od => {
                            od.OrderId = int.Parse(data);
                            od.orderType = "Outbound";
                        });
                        HttpResponseMessage resOD = await client.PostAsJsonAsync($"{urlOD}/Add", OrderItems);

                        if (resOD.IsSuccessStatusCode) {
                            return RedirectToAction("Index", "Outbound");
                        } else {
                            ModelState.AddModelError("", await resOD.Content.ReadAsStringAsync());
                            return View("Create", ConvertToOrder(null, order));
                        }
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Create", ConvertToOrder(null, order));
                    }
                }

            }
        }

        public async Task<IActionResult> Edit(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            Order order = new Order();
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync($"{urlOrder}/{id}")) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        order = JsonConvert.DeserializeObject<Order>(data);
                    }
                }
            }

            var listUser = await LoadDataAsync<User>(urlUser);

            ViewBag.totalCost = order?.OrderDetails.Sum(od => od.TotalPrice) ?? 0;
            ViewBag.totalProduct = order?.OrderDetails.Count ?? 0;
            ViewBag.Customer = await LoadDataAsync<Customer>(urlCustomer);
            ViewBag.Users = listUser.Where(x => x.Role != 1).ToList();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDTO order, List<OrderDetailDTO> OrderItems) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            order.OrderType = "Outbound";
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PutAsJsonAsync($"{urlOrder}/{id}", order)) {
                    if (res.IsSuccessStatusCode) {
                        HttpResponseMessage resDelete = await client.DeleteAsync($"{urlOD}/Order/{id}");

                        if (resDelete.IsSuccessStatusCode) {
                            OrderItems.ForEach(od => {
                                od.OrderId = id;
                                od.orderType = "Outbound";
                            });
                            HttpResponseMessage resOD = await client.PostAsJsonAsync($"{urlOD}/Add", OrderItems);

                            if (resOD.IsSuccessStatusCode) {
                                using (var getResponse = await client.GetAsync($"{urlOrder}/{id}")) {
                                    if (getResponse.IsSuccessStatusCode) {
                                        var jsonString = await getResponse.Content.ReadAsStringAsync();
                                        var updatedOrder = JsonConvert.DeserializeObject<Order>(jsonString);
                                        ViewBag.totalCost = updatedOrder?.OrderDetails.Sum(od => od.TotalPrice) ?? 0;
                                        ViewBag.totalProduct = updatedOrder?.OrderDetails.Count ?? 0;
                                        return View("Details", updatedOrder);
                                    }
                                }
                            } else {
                                ModelState.AddModelError("", await resOD.Content.ReadAsStringAsync());
                            }
                        } else {
                            ModelState.AddModelError("", await resDelete.Content.ReadAsStringAsync());
                        }
                    } else {
                        ModelState.AddModelError("", await res.Content.ReadAsStringAsync());
                    }
                }
            }
            return View("Edit", ConvertToOrder(id, order));
        }

        public async Task<IActionResult> ExportToExcel() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            try {
                using (HttpClient client = new HttpClient()) {
                    using (HttpResponseMessage res = await client.GetAsync($"{urlOrder}/export/Outbound")) {
                        using (HttpContent content = res.Content) {
                            if (res.IsSuccessStatusCode) {
                                var fileBytes = await res.Content.ReadAsByteArrayAsync();

                                // Get the filename from the Content-Disposition header
                                var contentDisposition = res.Content.Headers.ContentDisposition;
                                string fileName = contentDisposition != null &&
                                                    !string.IsNullOrEmpty(contentDisposition.FileName)
                                                    ? contentDisposition.FileName.Trim('"')
                                                    : "DanhSachDonHang.xlsx";

                                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                            } else {
                                return RedirectToAction("Index", new { errorMessage = "Failed to export data" });
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                return RedirectToAction("Index", new { errorMessage = $"Error: {ex.Message}" });
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

        private Order ConvertToOrder(int? id, OrderDTO dto) {
            return new Order {
                OrderId = id ?? 0,
                CustomerId = dto.CustomerId,
                SupplierId = dto.SupplierId,
                OrderType = dto.OrderType,
            };
        }

    }
}
