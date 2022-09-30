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
using Microsoft.Extensions.Logging;
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
        ILogger _logger;

        public ProductsController(
            IProductService productService,
            IWeatherService weatherService,
            IHttpContextAccessor contextAccessor,
            ILogger<ProductsController> logger
            )
        {
            _productService = productService;
            _weatherService = weatherService;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        // GET http://localhost:5000/Products
        public async Task<IActionResult> Index()
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var client = new HttpClient();
            //client.SetBearerToken(accessToken);
            //var response = await client.GetStringAsync("http://localhost:5100/api/Products/GetProducts");
            //return Ok(response);

            _logger.LogWarning("log from mvc {d}", DateTime.Now);
            var products = await _productService.GetProducts();
            return Ok(products);


        }

        public async Task<IActionResult> Weather()
        {
            /*
            var (weather, error) = await _weatherService.Get().TryCatch();
            if (error != null)
                return Ok(error);
            else
                return Ok(weather); */

            var weather = await _weatherService.Get();

            return Ok(weather);

        }

        // GET http://localhost:5000/Products/ThrowException
        public async Task<IActionResult> ThrowException()
        {
            var result = await _productService.ThrowException();

            return Ok(result);
        }
    }
}