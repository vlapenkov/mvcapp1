using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using WebApi1.Contracts.Dto;
using WebApi1.Contracts.Interfaces;

namespace mvcapp.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        IProductService _productService;
        IWeatherService _weatherService;
        IHttpContextAccessor _contextAccessor;

        public ProductsController(IProductService productService, IWeatherService weatherService, IHttpContextAccessor contextAccessor)
        {
            _productService = productService;
            _weatherService = weatherService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var client = new HttpClient();
            //client.SetBearerToken(accessToken);
            //var response = await client.GetStringAsync("http://localhost:5100/api/Products/GetProducts");
            //return Ok(response);

            // _contextAccessor
            var products = await _productService.GetProducts();
            return Ok(products);


        }

        public async Task<IActionResult> Weather()
        {

            var (weather, error) = await _weatherService.Get().TryCatch();
            if (error != null)
                return Ok(error);
            else
                return Ok(weather);
        }
    }
}