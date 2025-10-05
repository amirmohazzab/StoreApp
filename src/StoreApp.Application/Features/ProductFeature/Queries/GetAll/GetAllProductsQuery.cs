using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.GetAll
{
    public class GetAllProductsQuery : RequestParametersBasic, IRequest<PaginationResponse<ProductDto>>, ICashQuery
    {
        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public int HoursSaveData => 1;
    }
}
