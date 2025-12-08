using AutoMapper;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductCategoryDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Queries.GetAll
{
    public class GetProductCategoriesQuery : IRequest<List<ProductCategoryDto>> { }

    public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, List<ProductCategoryDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductCategoriesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<ProductCategoryDto>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await uow.Repository<ProductCategory>().GetAllAsync(cancellationToken);

            return mapper.Map<List<ProductCategoryDto>>(categories);
        }
    }
}
