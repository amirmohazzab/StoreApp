using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductBrandFeature.Commands.UpdatePrpductBrand
{
    public class UpdateProductBrandCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }
    }

    internal class UpdateProductBrandCommandHandler : IRequestHandler<UpdateProductBrandCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public UpdateProductBrandCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(UpdateProductBrandCommand request, CancellationToken cancellationToken)
        {
            var productBrand = await uow.Repository<ProductBrand>().GetByIdAsync(request.Id, cancellationToken);
            if (productBrand == null)
                return false;

            if (productBrand.Title == request.Title && productBrand.Description == request.Description && productBrand.Summary == request.Summary)
                return true;

            productBrand.Title = request.Title;
            productBrand.Description = request.Description;
            productBrand.Summary = request.Summary;

            uow.Repository<ProductBrand>().Update(productBrand);
            var result = await uow.Save(cancellationToken);

            return result > 0;
        }
    }
}
