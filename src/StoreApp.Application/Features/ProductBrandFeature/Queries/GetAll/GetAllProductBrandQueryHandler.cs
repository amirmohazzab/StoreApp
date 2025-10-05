using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductBrandFeature.Queries.GetAll
{
    public class GetAllProductBrandQueryHandler : IRequestHandler<GetAllProductBrandQuery, IEnumerable<ProductBrand>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllProductBrandQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductBrand>> Handle(GetAllProductBrandQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetProductBrandSpec();
            //return await unitOfWork.Repository<ProductBrand>().ListAsyncSpec(spec, cancellationToken);
            return await unitOfWork.Repository<ProductBrand>().GetAllAsync(cancellationToken);
        }
    }
}
