using MongoDB.Driver;

namespace backend.Data;

public class MongoDbService
{
    private readonly IMongoDatabase _db;

    public MongoDbService(IConfiguration configuration)
    {
        // Use "DbConnection" key as defined in the configuration file.
        var connectionString = configuration.GetConnectionString("DbConnection")
                               ?? throw new ArgumentNullException(nameof(configuration), "MongoDB connection string is not configured");

        try
        {
            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(connectionString);
            _db = client.GetDatabase(mongoUrl.DatabaseName);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to initialize MongoDB connection", ex);
        }
    }

    public IMongoDatabase Database => _db;
}