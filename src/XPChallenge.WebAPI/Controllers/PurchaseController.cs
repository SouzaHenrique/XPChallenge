using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using XPChallenge.Application.Commom.Events;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;
using XPChallenge.Application.Features.Customer.Commands;

#pragma warning disable CA2007

namespace XPChallenge.WebAPI.Controllers;

[Route("/purchases")]
public class PurchaseController : Controller
{
    [HttpPost]

    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    [SwaggerOperation(OperationId = "Purchases_Post")]
    public async Task<IActionResult> Post([FromBody] PurchaseFinancialProductCommand request,
                                          [FromServices] ISender sender, [FromServices] IPublisher publisher,
                                          [FromServices] IFinancialProductRepository financialProductRepository,
                                          [FromServices] ICustomerRepository customerRepository,
                                          CancellationToken cancellationToken)
    {
        var product = await financialProductRepository.GetById(request.FinancialProductId, cancellationToken);
        var customer = await customerRepository.GetById(request.CustomerId, cancellationToken);

        if (product is null || customer is null)
            return NotFound();


        var response = await sender.Send(request);
        await publisher.Publish(new PurchaseMadeNotification()
        {
            CustomerId = request.CustomerId,
            FinancialProductID = request.FinancialProductId,
            Quantity = request.Quantity,
            UnitPrice = product.CurrentPurchasePrice
        });

        return new CreatedResult("", response);
    }
}
