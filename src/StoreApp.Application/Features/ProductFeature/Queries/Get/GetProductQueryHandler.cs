using AutoMapper;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.ProductFeature.Queries.GetAll;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.Get
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetProductSpec(request.Id);
            var result = await unitOfWork.Repository<Product>().GetEntityWithSpec(spec, cancellationToken);
            if (result == null) throw new NotFoundEntityException();
            return mapper.Map<ProductDto>(result);

            //var entity = await unitOfWork.Repository<Product>().GetByIdAsync(request.Id, cancellationToken);
            //if (entity == null) throw new Exception(message: "error message");
            //return mapper.Map<ProductDto>(entity);
        }
    }
}
