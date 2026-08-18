using CustomerOrder.Core.Enums;
using System;
using System.Collections.Generic;

namespace CustomerOrder.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public virtual ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    }
}
