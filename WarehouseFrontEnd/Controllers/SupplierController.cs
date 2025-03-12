using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;

namespace WarehouseFrontEnd.Controllers
{
    public class SupplierController : Controller
    {
        private readonly string urlSupplier = "https://localhost:5100/api/Suppliers";

        public async Task<IActionResult> Index(string? query)
        {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            else
            {
                ViewBag.CurrentUser = current_user;
            }

            List<Supplier> list = new List<Supplier>();
            if (query != null)
            {
                list = await LoadDataAsync<Supplier>($"{urlSupplier}?search={query}");
            }
            else
            {
                list = await LoadDataAsync<Supplier>(urlSupplier);
            }
            ViewBag.SearchValue = query ?? "";
            ViewBag.Suppliers = list;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            else
            {
                ViewBag.CurrentUser = current_user;
            }

            Supplier supplier = new Supplier();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync($"{urlSupplier}/{id}"))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        supplier = JsonConvert.DeserializeObject<Supplier>(data);
                    }
                }
            }
            return View(supplier);
        }

        public async Task<IActionResult> Create()
        {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            else
            {
                ViewBag.CurrentUser = current_user;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierDTO supplier)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(urlSupplier, supplier))
                {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Supplier");
                    }
                    else
                    {
                        ModelState.AddModelError("", data);
                        return View("Create", ConvertToSupplier(null, supplier));
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierDTO supplier)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PutAsJsonAsync($"{urlSupplier}/{id}", supplier))
                {
                    var data = await res.Content.ReadAsStringAsync();
                    if (res.IsSuccessStatusCode)
                    {
                        return View("Details", ConvertToSupplier(id, supplier));
                    }
                    else
                    {
                        ModelState.AddModelError("", data);
                        return View("Details", ConvertToSupplier(null, supplier));
                    }
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.DeleteAsync($"{urlSupplier}/{id}");
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Supplier");
                }
                else
                {
                    Supplier supplier = new Supplier();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    using (var getResponse = await client.GetAsync($"{urlSupplier}/{id}"))
                    {
                        if (getResponse.IsSuccessStatusCode)
                        {
                            var jsonString = await getResponse.Content.ReadAsStringAsync();
                            supplier = JsonConvert.DeserializeObject<Supplier>(jsonString);
                        }
                    }
                    return View("Details", supplier);
                }
            }
        }

        //=======================================================================================================
        private async Task<List<T>> LoadDataAsync<T>(string url)
        {
            List<T> dataList = new List<T>();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        dataList = JsonConvert.DeserializeObject<List<T>>(data);
                    }
                }
            }
            return dataList;
        }

        private Supplier ConvertToSupplier(int? id, SupplierDTO dto)
        {
            return new Supplier
            {
                SupplierId = id ?? 0,
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
            };
        }
    }
}
