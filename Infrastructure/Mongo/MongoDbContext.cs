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

public class MotoDocument
{
    public int Id { get; set; }
    public string Placa { get; set; } = null!;
    public string Marca { get; set; } = null!;
    public string Modelo { get; set; } = null!;
    public int Ano { get; set; }
}

public class VagaDocument
{
    public int Id { get; set; }
    public string Status { get; set; } = "Livre";
    public int Numero { get; set; }
    public string Patio { get; set; } = null!;
}

public class CounterDocument
{
    public string Name { get; set; } = null!; 
    public int Value { get; set; }
}