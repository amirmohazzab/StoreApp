using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductBrandFeature.Commands.CreateProductBrand
{
    public class CreateProductBrandCommand : IRequest<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }
    }

    public class CreateProductBrandCommandHandler : IRequestHandler<CreateProductBrandCommand, int>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public CreateProductBrandCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateProductBrandCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductBrand
            {
                Title = request.Title,
                Description = request.Description,
                Summary = request.Summary,
                CreatedBy = currentUserService.UserId
            };

            await uow.Repository<ProductBrand>().AddAsync(entity, cancellationToken);
            await uow.Save(cancellationToken);

            return entity.Id;
        }
    }
}
