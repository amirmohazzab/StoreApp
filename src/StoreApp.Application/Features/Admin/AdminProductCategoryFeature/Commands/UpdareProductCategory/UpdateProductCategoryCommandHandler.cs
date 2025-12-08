using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Commands.UpdareProductCategory
{
    public class UpdateProductCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public UpdateProductCategoryCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await uow.Repository<ProductCategory>().GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return false;

            if (category.Name == request.Name)
                return true;

            category.Name = request.Name;

            uow.Repository<ProductCategory>().Update(category);
            var result = await uow.Save(cancellationToken);

            return result > 0;
        }
    }
}
