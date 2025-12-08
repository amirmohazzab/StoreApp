using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductFeature.Commands.DeleteProduct
{
    public class AdminDeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public AdminDeleteProductCommand(int id)
        {
            Id = id;
        }
    }

    public class AdminDeleteProductCommandHandler : IRequestHandler<AdminDeleteProductCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUploadService uploadService;

        public AdminDeleteProductCommandHandler(IUnitOfWork unitOfWork, IUploadService uploadService)
        {
            this.unitOfWork = unitOfWork;
            this.uploadService = uploadService;
        }

        public async Task<bool> Handle(AdminDeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>().GetQueryable()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
                return false;

            product.IsDelete = true;
            var result = await unitOfWork.Save(cancellationToken);

            return result > 0;
        }
    }
}
