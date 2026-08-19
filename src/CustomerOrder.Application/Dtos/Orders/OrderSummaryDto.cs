using CustomerOrder.Core.Enums;
using System;

namespace CustomerOrder.Application.Dtos.Orders
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
