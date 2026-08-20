using CustomerOrder.Application.Dtos.Customers;
using CustomerOrder.Application.Dtos.Orders;
using CustomerOrder.Application.Interfaces;
using CustomerOrder.Core.Entities;
using CustomerOrder.Core.Exceptions;
using CustomerOrder.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderSummaryDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
        {
            var orderNumber = dto.OrderNumber.Trim();
            if (await _unitOfWork.Orders.OrderNumberExistsAsync(orderNumber, null, cancellationToken))
            {
                throw new ConflictException("An order with this order number already exists.");
            }

            if (dto.CustomerIds == null || dto.CustomerIds.Count == 0)
            {
                throw new InputValidationException("At least one customer is required for an order.");
            }

            var customers = new List<Customer>();
            foreach (var customerId in dto.CustomerIds)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    throw new NotFoundException("Customer");
                }
                customers.Add(customer);
            }

            var order = new Order
            {
                OrderNumber = orderNumber,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                Customers = customers
            };

            _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderSummaryDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status
            };
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OrderSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var orders = await _unitOfWork.Orders.ListAsync(cancellationToken);

            return orders
                .Select(order => new OrderSummaryDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status
                })
                .ToList();
        }

        public async Task<OrderDetailsDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _unitOfWork.Orders.GetWithCustomersAsync(id, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            return new OrderDetailsDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Customers = order.Customers.Select(customer => new CustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Email = customer.Email,
                    Phone = customer.Phone
                }).ToList()
            };
        }

        public async Task UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken = default)
        {
            var order = await _unitOfWork.Orders.GetWithCustomersAsync(id, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            var orderNumber = dto.OrderNumber.Trim();
            if (await _unitOfWork.Orders.OrderNumberExistsAsync(orderNumber, id, cancellationToken))
            {
                throw new ConflictException("An order with this order number already exists.");
            }

            if (dto.CustomerIds == null || dto.CustomerIds.Count == 0)
            {
                throw new InputValidationException("At least one customer is required for an order.");
            }

            var customers = new List<Customer>();
            foreach (var customerId in dto.CustomerIds)
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    throw new NotFoundException("Customer");
                }
                customers.Add(customer);
            }

            order.OrderNumber = orderNumber;
            order.OrderDate = dto.OrderDate;
            order.TotalAmount = dto.TotalAmount;
            order.Status = dto.Status;
            order.Customers = customers;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
