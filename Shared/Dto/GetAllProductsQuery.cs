﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dto
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
       
    }
}
