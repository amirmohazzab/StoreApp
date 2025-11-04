using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Features.ProductBrandFeature.Queries.GetAll;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductTypeFeature.Queries.GetAll
{
    public class GetAllProductTypeQueryHandler : IRequestHandler<GetAllProductTypeQuery, IEnumerable<ProductType>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllProductTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductType>> Handle(GetAllProductTypeQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetProductTypeSpec();
            return await unitOfWork.Repository<ProductType>().ListAsyncSpec(spec, cancellationToken);
            //return await unitOfWork.Repository<ProductType>().GetAllAsync(cancellationToken);
        }
    }
}
