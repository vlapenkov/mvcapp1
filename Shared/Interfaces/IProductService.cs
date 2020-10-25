using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
   public interface IProductService
    {
        [Get("/api/products/getproducts")]
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
