using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using XPChallenge.Application.Features.Customer.Commands;

namespace XPChallenge.WebAPI.Controllers;

#pragma warning disable CA2007


[Route("/customers")]
public class CustomerController : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [SwaggerOperation(OperationId = "Customers_Post")]
    public async Task<IActionResult> Post([FromBody] CreateCustomerCommand request, [FromServices] ISender sender)
    {
        var response = await sender.Send(request);
        return new CreatedResult("", response);
    }
}
