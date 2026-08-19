using CustomerOrder.Application.Dtos.Orders;
using System.Collections.Generic;

namespace CustomerOrder.Application.Dtos.Customers
{
    public class CustomerDetailsDto : CustomerDto
    {
        public IReadOnlyList<OrderSummaryDto> Orders { get; set; } = new List<OrderSummaryDto>();
    }
}
