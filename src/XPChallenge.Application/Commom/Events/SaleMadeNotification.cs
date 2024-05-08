using MediatR;
using XPChallenge.Application.Features.FinancialTransaction.Commands;

namespace XPChallenge.Application.Commom.Events;
public class SaleMadeNotification : INotification
{
    public Guid FinancialProductID { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public Domain.Enums.TransactionTypeEnum TransactionType = Domain.Enums.TransactionTypeEnum.Sale;
    public Guid CustomerId { get; set; }

    public CreateFinancialTransactionCommand MapToCommand()
    {
        return new CreateFinancialTransactionCommand()
        {
            FinancialProductID = FinancialProductID,
            Quantity = Quantity,
            UnitPrice = UnitPrice,
            TransactionType = TransactionType,
            CustomerId = CustomerId
        };
    }

    public class SaleMadeNotificationHandler(ISender sender) : INotificationHandler<SaleMadeNotification>
    {
        public async Task Handle(SaleMadeNotification notification, CancellationToken cancellationToken)
        {
            var createSaleCommand = notification.MapToCommand();
            await sender.Send(createSaleCommand);
        }
    }
}
