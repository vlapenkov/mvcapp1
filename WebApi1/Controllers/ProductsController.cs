using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public class ProductsController : ControllerBase
    {
        ProductsDbContext _dbContext;
        public ProductsController(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await Task.FromResult<IEnumerable<ProductDto>>(products);
         
           
        }

        private IEnumerable<ProductDto> products 
            { get {

                return _dbContext.Products.Select(p => new ProductDto { Id = p.Id, Name = p.Name }).ToList();
            //yield return new ProductDto { Id = 1, Name = "First" };
            //yield return new ProductDto { Id = 2, Name = "Second" };
            }
        }
    }
}