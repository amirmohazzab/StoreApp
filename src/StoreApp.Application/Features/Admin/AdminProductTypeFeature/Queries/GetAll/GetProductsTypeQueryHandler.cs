using AutoMapper;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductCategoryDto;
using StoreApp.Application.Dtos.Admin.AdminProductTypeDtp;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductTypeFeature.Queries.GetAll
{
    public class GetProductTypeQuery : IRequest<List<ProductTypeDto>> { }

    public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductTypeQuery, List<ProductTypeDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductCategoriesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<ProductTypeDto>> Handle(GetProductTypeQuery request, CancellationToken cancellationToken)
        {
            var productTypes= await uow.Repository<ProductType>().GetAllAsync(cancellationToken);

            return mapper.Map<List<ProductTypeDto>>(productTypes);
        }
    }
}
