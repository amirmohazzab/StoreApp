using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Commands.UpdateLikeCount
{
    public class UpdateProductLikeCountCommand: IRequest<int>
    {
        public int ProductId { get; set; }

        public bool IsLiked { get; set; }
    }

    public class UpdateProductLikeCountCommandHandler : IRequestHandler<UpdateProductLikeCountCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateProductLikeCountCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateProductLikeCountCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>()
         .GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null) return 0;

            if (request.IsLiked)
                product.LikeCount++;
            else
                product.LikeCount--;

            if (product.LikeCount < 0)
                product.LikeCount = 0;

            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Save(cancellationToken);

            return product.LikeCount; 
        }
    }
}
