using StoreApp.Application.Contracts.Specification;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.GetAll
{
    public class GetProductSpec : BaseSpecification<Product>
    {
        public GetProductSpec(GetAllProductsQuery specParams) : base(Expression.ExpressionSpec(specParams))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            if (specParams.TypeSort == TypeSort.Desc)
            {
                switch (specParams.Sort)
                {
                    case 1:
                        AddOrderByDesc(x => x.Title);
                        break;
                    case 2:
                        AddOrderByDesc(x => x.ProductType.Title);
                        break;
                    case 3:
                        AddOrderByDesc(x => x.Price);
                        break;
                    default:
                        AddOrderByDesc(x => x.Title);
                        break;
                }
            }
            else
            {
                switch (specParams.Sort)
                {
                    case 1:
                        AddOrderBy(x => x.Title);
                        break;
                    case 2:
                        AddOrderBy(x => x.ProductType.Title);
                        break;
                    case 3:
                        AddOrderBy(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Title);
                        break;
                }
            }
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize, true);

        }

        public GetProductSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }

    public static class Expression
    {
        public static Expression<Func<Product, bool>> ExpressionSpec(GetAllProductsQuery specParams)
        {
            return x =>
                (string.IsNullOrEmpty(specParams.Search) || x.Title.ToLower().Contains(specParams.Search))
                && (!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId.Value)
                && (!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId.Value);
        }
    }

    public class ProductCountSpec : BaseSpecification<Product>
    {
        public ProductCountSpec(GetAllProductsQuery specParams) : base(Expression.ExpressionSpec(specParams))
        {
            IsPagingEnabled = false;
        }
    }
}
