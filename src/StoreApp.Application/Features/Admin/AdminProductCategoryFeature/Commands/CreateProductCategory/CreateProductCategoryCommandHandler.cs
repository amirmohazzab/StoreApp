using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, int>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public CreateProductCategoryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductCategory
            {
                Name = request.Name,
                CreatedBy = currentUserService.UserId
            };

            await uow.Repository<ProductCategory>().AddAsync(entity, cancellationToken);
            await uow.Save(cancellationToken);

            return entity.Id;
        }
    }
}
