using CustomerOrder.Api.Filters;
using CustomerOrder.Application.Common;
using CustomerOrder.Application.Dtos.Orders;
using CustomerOrder.Application.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CustomerOrder.Api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IReadOnlyList<OrderSummaryDto>>.SuccessResponse(
                orders, "Orders retrieved successfully"));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponse<OrderDetailsDto>.SuccessResponse(
                order, "Order retrieved successfully"));
        }

        //[ApiAuthorize(Roles = "admin")]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(CreateOrderDto request, CancellationToken cancellationToken)
        {
            var order = await _orderService.CreateAsync(request, cancellationToken);

            return Created(
                "api/orders/" + order.Id,
                ApiResponse<OrderSummaryDto>.SuccessResponse(order, "Order created successfully"));
        }

        //[ApiAuthorize(Roles = "admin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, UpdateOrderDto request, CancellationToken cancellationToken)
        {
            await _orderService.UpdateAsync(id, request, cancellationToken);

            return Ok(ApiResponse.SuccessResponse("Order updated successfully"));
        }

        [ApiAuthorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _orderService.DeleteAsync(id, cancellationToken);

            return Ok(ApiResponse.SuccessResponse("Order deleted successfully"));
        }
    }
}
