using CustomerOrder.Api.Filters;
using CustomerOrder.Application.Common;
using CustomerOrder.Application.Dtos.Customers;
using CustomerOrder.Application.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CustomerOrder.Api.Controllers
{
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly ICustomerService   _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [ApiAuthorize(Roles = "admin")]
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IReadOnlyList<CustomerDto>>.SuccessResponse(
                customers, "Customers retrieved successfully"));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponse<CustomerDetailsDto>.SuccessResponse(
                customer, "Customer retrieved successfully"));
        }

        [ApiAuthorize(Roles = "admin")]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(CreateCustomerDto request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.CreateAsync(request, cancellationToken);

            return Created(
                "api/customers/" + customer.Id,
                ApiResponse<CustomerDto>.SuccessResponse(customer, "Customer created successfully"));
        }

        [ApiAuthorize(Roles = "admin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, UpdateCustomerDto request, CancellationToken cancellationToken)
        {
            await _customerService.UpdateAsync(id, request, cancellationToken);

            return Ok(ApiResponse.SuccessResponse("Customer updated successfully"));
        }

        [ApiAuthorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _customerService.DeleteAsync(id, cancellationToken);

            return Ok(ApiResponse.SuccessResponse("Customer deleted successfully"));
        }
    }
}
