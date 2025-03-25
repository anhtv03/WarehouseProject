using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;

namespace WarehouseFrontEnd.Controllers {
    public class UserController : Controller {
        private readonly string url = "https://localhost:5100/api/Users";


        public async Task<IActionResult> Index() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;

            }

            ViewBag.Users = await LoadDataAsync<UserViewDTO>(url);
            return View();
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
        public async Task<IActionResult> Create(RegisterDTO user) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(url + "/Register", user)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        return RedirectToAction("Index", "User");
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Create", ConvertToSupplier(null, user));
                    }
                }
            }
        }

        public async Task<IActionResult> Details(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            UserViewDTO user = new UserViewDTO();

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync($"{url}/{id}")) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<UserViewDTO>(data);
                    }
                }
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDTO user) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage res = await client.PatchAsync($"{url}/{id}", content)) {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode) {
                        return View("Details", ConvertToUser(id, user));
                    } else {
                        ModelState.AddModelError("", data);
                        return View("Details", ConvertToUser(null, user));
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
                HttpResponseMessage res = await client.DeleteAsync($"{url}/{id}");
                if (res.IsSuccessStatusCode) {
                    return RedirectToAction("Index", "User");
                } else {
                    UserViewDTO user = new UserViewDTO();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    using (var getResponse = await client.GetAsync($"{url}/{id}")) {
                        if (getResponse.IsSuccessStatusCode) {
                            var jsonString = await getResponse.Content.ReadAsStringAsync();
                            user = JsonConvert.DeserializeObject<UserViewDTO>(jsonString);
                        }
                    }
                    return View("Details", user);
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

        private UserViewDTO ConvertToSupplier(int? id, RegisterDTO dto) {
            return new UserViewDTO {
                UserId = id ?? 0,
                FullName = dto.FullName,
                Username = dto.Username,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Role = dto.Role,
            };
        }
        
        private UserViewDTO ConvertToUser(int? id, UserDTO dto) {
            return new UserViewDTO {
                UserId = id ?? 0,
                FullName = dto.FullName,
                Username = dto.Username,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Role = dto.Role,
            };
        }

    }
}
