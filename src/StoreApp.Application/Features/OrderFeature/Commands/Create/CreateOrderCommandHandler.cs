using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.OrderDto;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.Basket;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Enums;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.OrderFeature.Commands.Create
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }

        public string BuyerPhoneNumber { get; set; }

        public PortalType PortalType { get; set; } = PortalType.none;

        public ShipToAddress ShipToAddress { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<CreateOrderCommandHandler> logger;

        public CreateOrderCommandHandler(IBasketRepository basketRepository, IConfiguration configuration,
        ICurrentUserService currentUserService, IMapper mapper, IUnitOfWork unitOfWork, ILogger<CreateOrderCommandHandler> logger)
        {
            this.basketRepository = basketRepository;
            this.configuration = configuration;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateOrderCommandHandler started");

            try
            {
                var basket = await basketRepository.GetBasketAsync(request.BasketId);

                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync(request.DeliveryMethodId, cancellationToken);

                //var amount = (int)(basket.CalculateOriginalPrice() + deliveryMethod.Price);

                //var payment = await new Payment(amount)
                //    .PaymentRequest("Sale Factor", configuration["Order:CallBack"], "", request.BuyerPhoneNumber);

                var result = await CreateOrder(request, cancellationToken, basket, deliveryMethod);

                //await basketRepository.DeleteBasketAsync(basket.Id);

                //var portal = new Portal(result.Id, result.PortalType, PaymentDataStatus.Pending, amount, null);
                //await unitOfWork.Repository<Portal>().AddAsync(portal, cancellationToken);

                await unitOfWork.Save(cancellationToken);

                var model = mapper.Map<OrderDto>(result);
                //model.Link = payment.Link;
                logger.LogInformation("CreateOrderCommandHandler finished successfully");
                return model;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error in CreateOrderCommandHandler");
                throw;
            }
        }

        private async Task<Order> CreateOrder(CreateOrderCommand request, CancellationToken cancellationToken,
                            CustomerBasket basket, DeliveryMethod deliveryMethod)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var itemOrder = new ProductItemOrdered(item.Id, item.ProductName, item.Brand, item.Type, item.PictureUrl);
                orderItems.Add(new OrderItem(itemOrder, item.Price, item.Quantity));
            }

            var order = new Order()
            {
                BuyerPhoneNumber = request.BuyerPhoneNumber,
                ShipToAddress = request.ShipToAddress,
                DeliveryMethod = deliveryMethod,
                OrderItems = orderItems,
                SubTotal = basket.CalculateOriginalPrice(),
                PortalType = PortalType.none,
                Authority = "NotApplicable",
                TrackingCode = "NotApplicable",
                CreatedBy = currentUserService.UserId,
                OrderStatus = OrderStatus.PaymentSuccess,
                IsFinally = true
            };

            var result = await unitOfWork.Repository<Order>().AddAsync(order, cancellationToken);
            if (result == null) throw new BadRequestEntityException("Your order failed, please try again");
            await unitOfWork.Save(cancellationToken);

            return result;
        }
    }
}
