﻿using AlchemicShop.BLL.DTO;
using AlchemicShop.BLL.Interfaces;
using AlchemicShop.WEB.Managers;
using AlchemicShop.WEB.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AlchemicShop.WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IMapper mapper,
            ICategoryService categoryService,
            IProductService productService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<ActionResult> GetProductList()
        {
            ViewBag.Categories = _mapper.Map<List<CategoryViewModel>>(
                (await _categoryService.GetCategories()).ToList());
            return View(_mapper.Map<List<ProductViewModel>>
                (await _productService.GetProducts()).ToList());

        }

        public async Task<ActionResult> CreateProduct()
        {
            if (HttpContext.User.Identity.Name == "Admin")
            {
                ViewBag.Categories = new SelectList(
                    _mapper.Map<IEnumerable<CategoryViewModel>>
                    (await _categoryService.GetCategories()), "Id", "Name");
                return View();
            }
            else
                return RedirectToAction(nameof(GetProductList));
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProduct(_mapper.Map<ProductDTO>(product));
                return RedirectToAction(nameof(GetProductList));
            }
            else
            {
                ViewBag.Categories = new SelectList(
                    _mapper.Map<IEnumerable<CategoryViewModel>>
                    (await _categoryService.GetCategories()), "Id", "Name");
                return View(product);
            }
        }

        public async Task<ActionResult> ProductEdit(int? id)
        {
            if (HttpContext.User.Identity.Name == "BritainKing")
            {
                ViewBag.Categories = new SelectList(
                _mapper.Map<IEnumerable<CategoryViewModel>>
                (await _categoryService.GetCategories()), "Id", "Name");
                return View(_mapper.Map<ProductViewModel>
                    (await _productService.GetProduct(id)));
            }
            else
                return RedirectToAction(nameof(GetProductList));

        }

        [HttpPost]
        public async Task<ActionResult> ProductEdit(ProductViewModel productView)
        {
            if (ModelState.IsValid)
            {
                await _productService.EditProduct(_mapper.Map<ProductDTO>(productView));
                return RedirectToAction(nameof(GetProductList));
            }
            else
            {
                ViewBag.Categories = new SelectList(
                    _mapper.Map<IEnumerable<CategoryViewModel>>
                    (await _categoryService.GetCategories()), "Id", "Name");
                return View(productView);
            }
        }

        public async Task<ActionResult> ProductDelete(int? id)
        {
            return View(_mapper.Map<ProductViewModel>
                (await _productService.GetProduct(id)));
        }

        [HttpPost]
        public async Task<ActionResult> ProductDelete(int? id, string name)
        {
            await _productService.Delete(id);
            return RedirectToAction(nameof(DeleteSuccess), new { deletingProduct = name });
        }

        public ActionResult DeleteSuccess(string deletingProduct)
        {
            ViewBag.Name = deletingProduct;
            return View();
        }

        public async Task<ActionResult> AddProduct(int? id)
        {
            var session = new SessionManager(HttpContext);
            var product = await _productService.GetProduct(id);
            session.AddProduct(_mapper.Map<ProductViewModel>(product));
            return RedirectToAction(nameof(GetProductList));
        }

        public ActionResult GetCart()
        {
            var session = new SessionManager(HttpContext);
            return View(session.GetOrCreateProductList());
        }
    }
}