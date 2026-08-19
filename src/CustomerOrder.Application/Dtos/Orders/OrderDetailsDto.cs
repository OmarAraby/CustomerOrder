using CustomerOrder.Application.Dtos.Customers;
using System.Collections.Generic;

namespace CustomerOrder.Application.Dtos.Orders
{
    public class OrderDetailsDto : OrderSummaryDto
    {
        public IReadOnlyList<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
    }
}
