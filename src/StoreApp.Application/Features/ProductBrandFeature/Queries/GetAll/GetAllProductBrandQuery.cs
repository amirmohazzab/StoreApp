using MediatR;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductBrandFeature.Queries.GetAll
{
    public class GetAllProductBrandQuery : IRequest<IEnumerable<ProductBrand>>
    {
    }
}
