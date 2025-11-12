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
    public class UpdateProductLikeCountCommand: IRequest<bool>
    {
        public int ProductId { get; set; }

        public bool IsLiked { get; set; }
    }

    public class UpdateProductLikeCountCommandHandler : IRequestHandler<UpdateProductLikeCountCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateProductLikeCountCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateProductLikeCountCommand request, CancellationToken cancellationToken)
        {
            var productRepo = unitOfWork.Repository<Product>();

            var product = await unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
                return false;

            // ✅ افزایش یا کاهش تعداد لایک بر اساس وضعیت جدید
            if (request.IsLiked)
                product.LikeCount += 1;
            else if (product.LikeCount > 0)
                product.LikeCount -= 1;

            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}
