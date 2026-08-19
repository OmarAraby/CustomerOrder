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

    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.Trim();
            if (await _unitOfWork.Customers.EmailExistsAsync(email, null, cancellationToken))
            {
                throw new ConflictException("A customer with this email already exists.");

            }
            var customer = new Customer
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Address = dto.Address.Trim(),
                Email = email,
                Phone = dto.Phone.Trim()
            };

            _unitOfWork.Customers.Add(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var customer = await _unitOfWork.Customers.GetWithOrdersAsync(id, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException("Customer");
            }

            foreach (var order in customer.Orders)
            {
                if (order.Customers.Count == 1)
                {
                    throw new ConflictException(
                        "This customer cannot be deleted because at least one of their orders would be left with no customers.");
                }
            }

            _unitOfWork.Customers.Remove(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var customers = await _unitOfWork.Customers.ListAsync(cancellationToken);

            return customers
                        .Select(customer => new CustomerDto
                        {
                            Id = customer.Id,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Address = customer.Address,
                            Email = customer.Email,
                            Phone = customer.Phone
                        })
                        .ToList();
        }

        public async Task<CustomerDetailsDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var customer = await _unitOfWork.Customers.GetWithOrdersAsync(id, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException("curtomer");

            }

            return new CustomerDetailsDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                Phone = customer.Phone,
                Orders = customer.Orders.Select(order => new OrderSummaryDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status
                }).ToList()
            };
        }

        public async Task UpdateAsync(int id, UpdateCustomerDto dto, CancellationToken cancellationToken = default)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException("Customer");
            }

            var email = dto.Email.Trim();
            if (await _unitOfWork.Customers.EmailExistsAsync(email, id, cancellationToken))
            {
                throw new ConflictException("A customer with this email already exists.");
            }

            customer.FirstName = dto.FirstName.Trim();
            customer.LastName = dto.LastName.Trim();
            customer.Address = dto.Address.Trim();
            customer.Email = email;
            customer.Phone = dto.Phone.Trim();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
