using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Commands.DeleteProductCategory
{
    public class DeleteProductCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public DeleteProductCategoryCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<bool> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await uow.Repository<ProductCategory>().GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                throw new Exception("Category not found");

            uow.Repository<ProductCategory>().Delete(category, cancellationToken);
            var result = await uow.Save(cancellationToken);

            return result > 0;
        }
    }
}
