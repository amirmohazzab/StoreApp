using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.CreateProductType
{
    public class CreateProductTypeCommand : IRequest<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }
    }

    public class CreateProductTypeCommandHandler : IRequestHandler<CreateProductTypeCommand, int>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public CreateProductTypeCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductType
            {
                Title = request.Title,
                Description = request.Description,
                Summary = request.Summary,
                CreatedBy = currentUserService.UserId
            };

            await uow.Repository<ProductType>().AddAsync(entity, cancellationToken);
            await uow.Save(cancellationToken);

            return entity.Id;
        }
    }
}
