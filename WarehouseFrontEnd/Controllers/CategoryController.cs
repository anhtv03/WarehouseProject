using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;

namespace WarehouseFrontEnd.Controllers {
    public class CategoryController : Controller {
        private readonly string urlCategory = "https://localhost:5100/api/Categories";

        public async Task<IActionResult> Index(string? query) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            List<Category> list = new List<Category>();
            if (query != null) {
                list = await LoadDataAsync<Category>($"{urlCategory}?search={query}");
            } else {
                list = await LoadDataAsync<Category>(urlCategory);
            }
            ViewBag.SearchValue = query ?? "";
            ViewBag.Categories = list;
            return View();
        }

        public async Task<IActionResult> Details(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            Category category = new Category();

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync($"{urlCategory}/{id}")) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        category = JsonConvert.DeserializeObject<Category>(data);
                    }
                }
            }
            return View(category);
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
        public async Task<IActionResult> Create(CategoryDTO category) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(urlCategory, category)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        return RedirectToAction("Index", "Category");
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Create", ConvertToCategory(null, category));
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDTO category) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PutAsJsonAsync($"{urlCategory}/{id}", category)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        return View("Details", ConvertToCategory(id, category));
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Details", ConvertToCategory(null, category));
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
                HttpResponseMessage res = await client.DeleteAsync($"{urlCategory}/{id}");
                if (res.IsSuccessStatusCode) {
                    return RedirectToAction("Index", "Category");
                } else {
                    Category category = new Category();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    using (var getResponse = await client.GetAsync($"{urlCategory}/{id}")) {
                        if (getResponse.IsSuccessStatusCode) {
                            var jsonString = await getResponse.Content.ReadAsStringAsync();
                            category = JsonConvert.DeserializeObject<Category>(jsonString);
                        }
                    }
                    return View("Details", category);
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

        private Category ConvertToCategory(int? id, CategoryDTO dto) {
            return new Category {
                CategoryId = id ?? 0,
                Name = dto.Name,
                Description = dto.Description,
            };
        }
    }
}
