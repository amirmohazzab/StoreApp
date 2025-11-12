using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Commands.IncrementViewCount
{
    public class IncrementProductViewCountCommand: IRequest<bool>
    {
        public int ProductId { get; set; }
    }

    public class IncrementProductViewCountCommandHandler : IRequestHandler<IncrementProductViewCountCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;

        public IncrementProductViewCountCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(IncrementProductViewCountCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
                return false;

            product.ViewCount += 1;

            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}
