using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.FinancialTransaction.Commands;
public class CreateFinancialTransactionCommand : IRequest<FinancialTransactionResponse>
{
    public Guid FinancialProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Domain.Enums.TransactionTypeEnum TransactionType { get; set; }
    public Guid CustomerId { get; set; }

    public Domain.Entities.FinancialTransaction MapToEntity()
    {
        return new Domain.Entities.FinancialTransaction(FinancialProductID, TransactionType,
                                                        Quantity, UnitPrice, DateTime.Now, CustomerId);
    }

    public class CreateFinancialTransactionCommandHandler(IFinancialTransactionRepository transactionRepository) : IRequestHandler<CreateFinancialTransactionCommand, FinancialTransactionResponse>
    {
        public async Task<FinancialTransactionResponse> Handle(CreateFinancialTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = request.MapToEntity();

            await transactionRepository.Add(transaction, cancellationToken);

            return new FinancialTransactionResponse { Id = transaction.Id };
        }
    }
}
