using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ElysiaAPI.Infrastructure.Mongo;

public class MongoSettings
{
    public string ConnectionString { get; set; } = "";
    public string Database { get; set; } = "";
}

public class MongoDbContext
{
    public IMongoDatabase Db { get; }

    public MongoDbContext(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        Db = client.GetDatabase(options.Value.Database);
    }

    public IMongoCollection<MotoDocument> Motos => Db.GetCollection<MotoDocument>("motos");
    public IMongoCollection<VagaDocument> Vagas => Db.GetCollection<VagaDocument>("vagas");
    public IMongoCollection<CounterDocument> Counters => Db.GetCollection<CounterDocument>("counters");
}