using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace XPChallenge.Infrastructure.Persistance;
public static class MongoClassMapers
{
    public static void RegisterMaps()
    {
#pragma warning disable CS0618
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        if (!BsonClassMap.IsClassMapRegistered(typeof(Customer)))
        {
            BsonClassMap.TryRegisterClassMap<Customer>(c =>
            {
                c.MapProperty(x => x.FirstName);
                c.MapProperty(x => x.LastName);
                c.MapProperty(x => x.Balance);
                c.MapProperty(x => x.PurchasedProducts);
                c.MapCreator(x => new Customer(x.Id, x.FirstName, x.LastName, x.Balance, x.PurchasedProducts));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(FinancialProduct)))
        {
            BsonClassMap.TryRegisterClassMap<FinancialProduct>(p =>
            {
                p.MapProperty(x => x.Name);
                p.MapProperty(x => x.CurrentPurchasePrice);
                p.MapProperty(x => x.DueDate);
                p.MapCreator(x => new FinancialProduct(x.Id, x.Name, x.CurrentPurchasePrice, x.DueDate));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(FinancialTransaction)))
        {
            BsonClassMap.TryRegisterClassMap<FinancialTransaction>(t =>
            {
                t.MapProperty(x => x.FinancialProductID);
                t.MapProperty(x => x.CustomerID);
                t.MapProperty(x => x.Quantity);
                t.MapProperty(x => x.UnitPrice);
                t.MapProperty(x => x.TransactionDate);
                t.MapProperty(x => x.TransactionType);
                t.MapCreator(x => new FinancialTransaction(x.Id, x.FinancialProductID, x.TransactionType, x.Quantity, x.UnitPrice, x.TransactionDate, x.CustomerID));
            });
        }
    }
}
