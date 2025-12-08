using AutoMapper;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductBrandDto;
using StoreApp.Application.Dtos.Admin.AdminProductTypeDtp;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductBrandFeature.Queries.GetAll
{
    public class GetProductBrandQuery : IRequest<List<ProductBrandDto>> { }

    public class GetProductBrandsQueryHandler : IRequestHandler<GetProductBrandQuery, List<ProductBrandDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetProductBrandsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<ProductBrandDto>> Handle(GetProductBrandQuery request, CancellationToken cancellationToken)
        {
            var productBrands = await uow.Repository<ProductBrand>().GetAllAsync(cancellationToken);

            return mapper.Map<List<ProductBrandDto>>(productBrands);
        }
    }
}
