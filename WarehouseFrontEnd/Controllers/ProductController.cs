using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WarehouseFrontEnd.Models.DTOs;
using WarehouseFrontEnd.Models.Entity;

namespace WarehouseFrontEnd.Controllers {
    public class ProductController : Controller {
        private readonly string urlProduct = "https://localhost:5100/api/Products";
        private readonly string urlCategory = "https://localhost:5100/api/Categories";
        private readonly string urlSupplier = "https://localhost:5100/api/Suppliers";

        public async Task<IActionResult> Index(string? query) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            List<Product> list = new List<Product>();
            if (query != null) {
                list = await LoadDataAsync<Product>($"{urlProduct}?search={query}");
            } else {
                list = await LoadDataAsync<Product>(urlProduct);
            }
            ViewBag.SearchValue = query ?? "";
            ViewBag.Products = list;
            return View();
        }


        public async Task<IActionResult> Details(int id) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            Product product = new Product();
            var categories = await LoadDataAsync<Category>(urlCategory);
            var suppliers = await LoadDataAsync<Supplier>(urlSupplier);

            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage res = await client.GetAsync($"{urlProduct}/{id}")) {
                    using (HttpContent content = res.Content) {
                        string data = await content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<Product>(data);
                    }
                }
            }

            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;
            return View(product);
        }

        public async Task<IActionResult> Create() {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            var categories = await LoadDataAsync<Category>(urlCategory);
            var suppliers = await LoadDataAsync<Supplier>(urlSupplier);
            
            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO product, IFormFile? file) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            using (HttpClient client = new HttpClient()) {
                using (var content = new MultipartFormDataContent()) {
                    content.Add(new StringContent(product.Name), "Name");
                    content.Add(new StringContent(product.Description == null ? "" : product.Description), "Description");
                    content.Add(new StringContent(product.Price.ToString()), "Price");
                    content.Add(new StringContent(product.CostPrice.ToString()), "CostPrice");
                    content.Add(new StringContent(product.Images ?? ""), "Images");

                    if (product.Quantity.HasValue) content.Add(new StringContent(product.Quantity.Value.ToString()), "Quantity");
                    if (product.AvailableQuantity.HasValue) content.Add(new StringContent(product.AvailableQuantity.Value.ToString()), "AvailableQuantity");
                    if (product.CategoryId.HasValue) content.Add(new StringContent(product.CategoryId.Value.ToString()), "CategoryId");
                    if (product.SupplierId.HasValue) content.Add(new StringContent(product.SupplierId.Value.ToString()), "SupplierId");

                    if (file != null && file.Length > 0) {
                        var stream = file.OpenReadStream();
                        content.Add(new StreamContent(stream), "file", file.FileName);
                    }

                    HttpResponseMessage response = await client.PostAsync($"{urlProduct}", content);
                    if (!response.IsSuccessStatusCode) {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", errorContent);
                        return View("Create", ConvertToProduct(null, product));
                    }
                }
            }


            //ViewBag.Categories = await LoadDataAsync<Category>(urlCategory);
            //ViewBag.Suppliers = await LoadDataAsync<Supplier>(urlSupplier);
            return RedirectToAction("Index", "Product");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDTO product, IFormFile? file) {
            UserViewDTO current_user = JsonConvert.DeserializeObject<UserViewDTO>(HttpContext.Session.GetString("User"));
            if (current_user == null) {
                return RedirectToAction("Index", "Auth");
            } else {
                ViewBag.CurrentUser = current_user;
            }

            var categories = await LoadDataAsync<Category>(urlCategory);
            var suppliers = await LoadDataAsync<Supplier>(urlSupplier);
            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;

            using (HttpClient client = new HttpClient()) {
                using (var content = new MultipartFormDataContent()) {
                    content.Add(new StringContent(product.Name), "Name");
                    content.Add(new StringContent(product.Description == null ? "" : product.Description), "Description");
                    content.Add(new StringContent(product.Price.ToString()), "Price");
                    content.Add(new StringContent(product.CostPrice.ToString()), "CostPrice");
                    content.Add(new StringContent(product.Images ?? ""), "Images");

                    if (product.Quantity.HasValue) content.Add(new StringContent(product.Quantity.Value.ToString()), "Quantity");
                    if (product.AvailableQuantity.HasValue) content.Add(new StringContent(product.AvailableQuantity.Value.ToString()), "AvailableQuantity");
                    if (product.CategoryId.HasValue) content.Add(new StringContent(product.CategoryId.Value.ToString()), "CategoryId");
                    if (product.SupplierId.HasValue) content.Add(new StringContent(product.SupplierId.Value.ToString()), "SupplierId");

                    if (file != null && file.Length > 0) {
                        var stream = file.OpenReadStream();
                        content.Add(new StreamContent(stream), "file", file.FileName);
                    }

                    HttpResponseMessage response = await client.PutAsync($"{urlProduct}/{id}", content);
                    if (!response.IsSuccessStatusCode) {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", errorContent);
                        return View("Details", ConvertToProduct(id, product));
                    }

                    using (var getResponse = await client.GetAsync($"{urlProduct}/{id}")) {
                        if (getResponse.IsSuccessStatusCode) {
                            var jsonString = await getResponse.Content.ReadAsStringAsync();
                            var updatedProduct = JsonConvert.DeserializeObject<Product>(jsonString);
                            return View("Details", updatedProduct);
                        }
                    }
                }
            }
            return View("Details", ConvertToProduct(id, product));
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int id) {
            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage res = await client.DeleteAsync($"{urlProduct}/{id}");
                if (res.IsSuccessStatusCode) {
                    return RedirectToAction("Index", "Product");
                } else {
                    Product product = new Product();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    using (var getResponse = await client.GetAsync($"{urlProduct}/{id}")) {
                        if (getResponse.IsSuccessStatusCode) {
                            var jsonString = await getResponse.Content.ReadAsStringAsync();
                            product = JsonConvert.DeserializeObject<Product>(jsonString);
                        }
                    }
                    return View("Details", product);
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

        private Product ConvertToProduct(int? id, ProductDTO dto) {
            return new Product {
                ProductId = id ?? 0,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CostPrice = dto.CostPrice,
                Quantity = dto.Quantity ?? 0,
                AvailableQuantity = dto.AvailableQuantity ?? 0,
                Images = dto.Images,
                CategoryId = dto.CategoryId ?? 0,
                SupplierId = dto.SupplierId ?? 0
            };
        }

    }
}
