using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;

namespace WarehouseFrontEnd.Controllers {
    public class CustomerController : Controller {
        private readonly string urlCustomer = "https://localhost:5100/api/Customers";

        public async Task<IActionResult> Index(string? query) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            List<Customer> list = new List<Customer>();
            if (query != null) {
                list = await LoadDataAsync<Customer>($"{urlCustomer}?search={query}");
            } else {
                list = await LoadDataAsync<Customer>(urlCustomer);
            }
            ViewBag.SearchValue = query ?? "";
            ViewBag.Customers = list;
            return View();
        }

        public async Task<IActionResult> Details(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            Customer customer = new Customer();

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync($"{urlCustomer}/{id}")) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        customer = JsonConvert.DeserializeObject<Customer>(data);
                    }
                }
            }
            return View(customer);
        }

        public async Task<IActionResult> Create() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerDTO customer) {
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(urlCustomer, customer)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        return RedirectToAction("Index", "Customer");
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Create", ConvertToCustomer(null, customer));
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerDTO customer) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PutAsJsonAsync($"{urlCustomer}/{id}", customer)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        return View("Details", ConvertToCustomer(id, customer));
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Details", ConvertToCustomer(null, customer));
                    }
                }
            }
        }

        public async Task<IActionResult> Delete(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage res = await client.DeleteAsync($"{urlCustomer}/{id}");
                if (res.IsSuccessStatusCode) {
                    return RedirectToAction("Index", "Customer");
                } else {
                    Customer customer = new Customer();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    using (var getResponse = await client.GetAsync($"{urlCustomer}/{id}")) {
                        if (getResponse.IsSuccessStatusCode) {
                            var jsonString = await getResponse.Content.ReadAsStringAsync();
                            customer = JsonConvert.DeserializeObject<Customer>(jsonString);
                        }
                    }
                    return View("Details", customer);
                }
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

        private Customer ConvertToCustomer(int? id, CustomerDTO dto) {
            return new Customer {
                CustomerId = id ?? 0,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Note = dto.Note
            };
        }
    }
}
