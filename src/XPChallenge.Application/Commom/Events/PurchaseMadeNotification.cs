using MediatR;
using XPChallenge.Application.Features.FinancialTransaction.Commands;

namespace XPChallenge.Application.Commom.Events;
public class PurchaseMadeNotification : INotification
{
    public Guid FinancialProductID { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public Domain.Enums.TransactionTypeEnum TransactionType = Domain.Enums.TransactionTypeEnum.Purchase;
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

    public class PurchaseMadeNotificationHandler(ISender sender) : INotificationHandler<PurchaseMadeNotification>
    {
        public async Task Handle(PurchaseMadeNotification notification, CancellationToken cancellationToken)
        {
            var createPurschaseCommand = notification.MapToCommand();
            await sender.Send(createPurschaseCommand);
        }
    }
}
