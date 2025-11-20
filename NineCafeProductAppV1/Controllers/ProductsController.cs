using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NineCafeProductAppV1.Constants;
using NineCafeProductAppV1.Models;
using NineCafeProductAppV1.Repositories;

namespace NineCafeProductAppV1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IRepository<ProductPosting> _repos;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(
            IRepository<ProductPosting> repos, 
            IWebHostEnvironment webHostEnvironment)
        {
            _repos = repos;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _repos.GetAllAsync();
            return View(products);
        }
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ImageUrl) &&
                (model.ImageFile == null || model.ImageFile.Length == 0))
            {
                ModelState.AddModelError(",", "Please provide an image URL or upload an image.");
                return View(model);
            }

            string finalImageUrl = "/images/products/default.png";

            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                finalImageUrl = model.ImageUrl;
            }
            else if(model.ImageFile?.Length > 0) 
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadFolder);
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadFolder, filename);
                
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                finalImageUrl = $"/images/products/{filename}";
            }

            var product = new ProductPosting
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Category = model.Category,
                IsActive = model.IsActive,
                ImageUrl = finalImageUrl,
                FoodPandaUrl = "https://www.foodpanda.com.kh/en/restaurant/q0xf/nine-cafe-tuol-sangke",
                PostedDate = DateTime.UtcNow
            };

            await _repos.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repos.GetByIdAsync(id);
            if (product == null) return NotFound();

            var model = new ProductViewModel
            {
                Id = id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                IsActive = product.IsActive,
                ImageUrl = product.ImageUrl
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (model.Id != id) return BadRequest();

            var existed = await _repos.GetByIdAsync(id);
            if (existed == null) return NotFound();

            string finalImageUrl = existed.ImageUrl;

            if(!string.IsNullOrEmpty(model.ImageUrl))
            {
                finalImageUrl = model.ImageUrl;
            }
            else if(model.ImageFile?.Length > 0)
            {
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadFolder);
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadFolder, filename);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                finalImageUrl = $"/images/products/{filename}";
            }

            existed.Title = model.Title;
            existed.Price = model.Price;
            existed.Description = model.Description;
            existed.Category = model.Category;
            existed.ImageUrl = finalImageUrl;
            existed.IsActive = model.IsActive;

            await _repos.UpdateAsync(existed);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repos.GetByIdAsync(id);
            if(product == null)  return NotFound();

            await _repos.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
