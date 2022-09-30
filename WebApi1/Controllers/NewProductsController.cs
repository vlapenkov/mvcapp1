using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi1.Contracts.Dto;

namespace WebApi1.Web.Controllers
{
    [Route("api/products/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes ="Bearer,Test")]
    [Authorize(Policy = "DefaultPolicy")]

    public class NewProductsController : ControllerBase
    {
        private IMediator _mediator;
        private ILogger _logger;
        private IHttpContextAccessor _contextAccessor;

        public NewProductsController(
            IMediator mediator,
            IHttpContextAccessor contextAccessor,
            ILogger<NewProductsController> logger)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }




        [HttpGet]
        [AllowAnonymous]
        [Authorize]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            _logger.LogWarning("log from webapi {d}", DateTime.Now);

            //throw new TneValidationException("exception message", "fio");
            var result = await _mediator.Send(new GetAllProductsQuery());

            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<string> ThrowException()
        {
            throw new TneErrorException("exception message");
        }

        [HttpGet]
        public async Task<ProductDto> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<int> Create(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpGet]
        public async Task<string> GetUser()
        {
            var name = User.Identity.Name;
            var email = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            return name;
        }


    }
}
