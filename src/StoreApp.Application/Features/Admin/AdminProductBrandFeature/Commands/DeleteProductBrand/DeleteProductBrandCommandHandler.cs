using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductBrandFeature.Commands.DeleteProductBrand
{
    public class DeleteProductBrandCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteProductBrandCommandHandler : IRequestHandler<DeleteProductBrandCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public DeleteProductBrandCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<bool> Handle(DeleteProductBrandCommand request, CancellationToken cancellationToken)
        {
            var productBrand = await uow.Repository<ProductBrand>().GetByIdAsync(request.Id, cancellationToken);
            if (productBrand == null)
                throw new Exception("Product Brand Type not found");

            uow.Repository<ProductBrand>().Delete(productBrand, cancellationToken);
            var result = await uow.Save(cancellationToken);

            return result > 0;
        }
    }
}
