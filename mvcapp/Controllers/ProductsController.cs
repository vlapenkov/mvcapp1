using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Interfaces;

namespace mvcapp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var client = new HttpClient();
            //client.SetBearerToken(accessToken);
            //var response = await client.GetStringAsync("http://localhost:5100/api/Products/GetProducts");
            //return Ok(response);
            var (products, error) = await _productService.GetProducts().TryCatch();
            if (error != null)
                return Ok(error);
            else
                return View(products);
        }
    }
}