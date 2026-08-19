using CustomerOrder.Core.Enums;
using System;
using System.Collections.Generic;

namespace CustomerOrder.Application.Dtos.Orders
{
    public class UpdateOrderDto
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<int> CustomerIds { get; set; }
    }
}
