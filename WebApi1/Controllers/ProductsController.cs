using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dto;

namespace WebApi1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public class ProductsController : ControllerBase
    {
        private IMediator _mediator;
       // private ProductsDbContext _dbContext;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        //    _dbContext = dbContext;
        }

        
              

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var result =await _mediator.Send(new GetAllProductsQuery());

            return result;
        }

        [HttpGet]
        public async Task<ProductDto> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return result;
        }


        [HttpPost]
        public async Task<int> Create(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            return result;
        }

        //private IEnumerable<ProductDto> products 
        //    { get {

        //        return _dbContext.Products.Select(p => new ProductDto { Id = p.Id, Name = p.Name }).ToList();
        //    //yield return new ProductDto { Id = 1, Name = "First" };
        //    //yield return new ProductDto { Id = 2, Name = "Second" };
        //    }
        //}
    }
}