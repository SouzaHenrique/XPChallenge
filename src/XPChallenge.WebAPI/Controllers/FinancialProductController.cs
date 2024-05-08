using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using XPChallenge.Application.Features.FinancialProduct.Commands;
using XPChallenge.Application.Features.FinancialProduct.Queries;
using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.WebAPI.Controllers;

#pragma warning disable CA2007


[Route("/products")]
public class FinancialProductController : Controller
{
    [HttpPost]

    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [SwaggerOperation(OperationId = "Products_Post")]
    public async Task<IActionResult> Post([FromBody] CreateProductCommand request, [FromServices] ISender sender)
    {
        var response = await sender.Send(request);
        return new CreatedResult("", response);
    }

    [HttpGet]

    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [SwaggerOperation(OperationId = "Products_GetExtract")]
    public async Task<ActionResult<ProductExtractValueObject>> GetProductExtract([FromQuery] GetProductExtractQuery request, [FromServices] ISender sender)
    {
        var response = await sender.Send(request);
        return Ok(response);
    }
}
