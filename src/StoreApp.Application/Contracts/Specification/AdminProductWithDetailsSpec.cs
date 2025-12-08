using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Contracts.Specification
{
    public class AdminProductWithDetailsSpec : BaseSpecification<Product>
    {
        public AdminProductWithDetailsSpec(int id)
        : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductImages);
            AddInclude(p => p.Colors);
            AddInclude(p => p.Sizes);
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            AddInclude(p => p.Category);
        }
    }
}
