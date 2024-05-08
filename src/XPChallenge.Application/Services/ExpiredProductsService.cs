using XPChallenge.Application.Commom.Contracts;
using XPChallenge.Application.Contracts;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Services;
public class ExpiredProductsService(IFinancialProductRepository financialProductRepository, INotificationService notificationService) : IExpiredProductsService
{
    public async Task CheckForExpiredProductsAndNotify(params string[] emails)
    {
        var allExpiredProducts = (await financialProductRepository.GetProductsByDueDate(DateTime.Now))?.ToList();

        var message = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var item in allExpiredProducts)
        {
            stringBuilder.AppendLine($"Product Name: {item.Name}, Due Date: {item.DueDate}");
        }

        if (allExpiredProducts.Any())
        {
            foreach (var email in emails)
            {
                await notificationService.SendEmailAsync(email, "Today Expired Products", stringBuilder.ToString());
            }
        }
    }
}
