using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUDWithRepository.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAll();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Product());
            }
            else
            {
                var product = await _productRepository.GetById(id);
                if (product != null)
                {
                    return View(product);
                }
                else
                {
                    TempData["ErrorMessage"] = $"Product details not found with id : {id}";
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult CreateOrEdit(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        _productRepository.Add(model);
                        TempData["SuccessMessage"] = "Product added successfully !";
                    }
                    else
                    {
                        _productRepository.Update(model);
                        TempData["SuccessMessage"] = "Product Updated successfully !";
                    }
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["ErrorMessage"] = "Model state is invalid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                throw;
            }
        }
    }
}
