using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.UpdateProductType
{
    public class UpdateProductTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }
    }

    internal class UpdateProductTypeCommandHandler : IRequestHandler<UpdateProductTypeCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public UpdateProductTypeCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var productType = await uow.Repository<ProductType>().GetByIdAsync(request.Id, cancellationToken);
            if (productType == null)
                return false;

            if (productType.Title == request.Title && productType.Description == request.Description && productType.Summary == request.Summary)
                return true;

            productType.Title = request.Title;
            productType.Description = request.Description;
            productType.Summary = request.Summary;

            uow.Repository<ProductType>().Update(productType);
            var result = await uow.Save(cancellationToken);

            return result > 0;
        }
    }
}
