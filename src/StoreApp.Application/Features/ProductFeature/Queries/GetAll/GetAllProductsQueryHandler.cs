using AutoMapper;
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
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginationResponse<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PaginationResponse<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetProductSpec(request);
            var count = await unitOfWork.Repository<Product>().CountAsyncSpec(new ProductCountSpec(request), cancellationToken);
            var result = await unitOfWork.Repository<Product>().ListAsyncSpec(spec, cancellationToken);
            var model = mapper.Map<IEnumerable<ProductDto>>(result);
            return new PaginationResponse<ProductDto>(request.PageIndex, request.PageSize, count, model);
            //return await unitOfWork.Repository<Product>().GetAllAsync(cancellationToken);
        }
    }
}
