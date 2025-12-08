using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.DeleteProductType
{
    public class DeleteProductTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public DeleteProductTypeCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<bool> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
        {
            var productType = await uow.Repository<ProductType>().GetByIdAsync(request.Id, cancellationToken);
            if (productType == null)
                throw new Exception("Product Type not found");

            await uow.Repository<ProductType>().Delete(productType, cancellationToken);
            var result = await uow.Save(cancellationToken);

            return result > 0;
        }
    }
}
