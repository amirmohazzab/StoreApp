using StoreApp.Application.Contracts.Specification;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductTypeFeature.Queries.GetAll
{
    public class GetProductTypeSpec : BaseSpecification<ProductType>
    {
        public GetProductTypeSpec()
        {
            IsPagingEnabled = false;
        }
    }
}
