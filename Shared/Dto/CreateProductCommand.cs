using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dto
{
    public class CreateProductCommand : IRequest<int>
    {
        public int Id { get; set; } // по идее id здесь не нужна если в сущности AutoIncrement
        public string Name { get; set; }
       
    }
}
