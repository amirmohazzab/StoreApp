using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 1,

        [EnumMember(Value = "PaymentSuccess")]
        PaymentSuccess,

        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed,

        [EnumMember(Value = "Shipped")]
        Shipped,

        [EnumMember(Value = "Delivered")]
        Delivered,

        [EnumMember(Value = "Cancelled")]
        Cancelled,
    }
}
