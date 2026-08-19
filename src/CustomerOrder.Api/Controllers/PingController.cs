using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using CustomerOrder.Core.Interfaces;

namespace CustomerOrder.Api.Controllers
{
  
    [RoutePrefix("api/ping")]
    public class PingController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get(CancellationToken cancellationToken)
        {
            var customers = await _unitOfWork.Customers.ListAsync(cancellationToken);
            var orders = await _unitOfWork.Orders.ListAsync(cancellationToken);

            return Ok(new
            {
                status = "ok",
                customers = customers.Count,
                orders = orders.Count
            });
        }
    }
}
