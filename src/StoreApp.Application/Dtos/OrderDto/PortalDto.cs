using StoreApp.Application.Common.Mapping;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.OrderDto
{
    public class PortalDto : IMapFrom<Portal>
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public PortalType Gateway { get; set; } 

        public PaymentDataStatus Status { get; set; } 

        public DateTime CreatedOn { get; set; } 

        public long Amount { get; set; }

        public string ReferenceId { get; set; }
    }
}
