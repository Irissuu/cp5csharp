using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ElysiaAPI.Infrastructure.Mongo;

[BsonIgnoreExtraElements]
public class MotoDocument
{
    [BsonId] public ObjectId MongoId { get; set; }
    public int Id { get; set; }
    public string Placa { get; set; } = null!;
    public string Marca { get; set; } = null!;
    public string Modelo { get; set; } = null!;
    public int Ano { get; set; }
}

[BsonIgnoreExtraElements]
public class VagaDocument
{
    [BsonId] public ObjectId MongoId { get; set; }
    public int Id { get; set; }
    public string Status { get; set; } = "Livre";
    public int Numero { get; set; }
    public string Patio { get; set; } = null!;
}

[BsonIgnoreExtraElements]
public class CounterDocument
{
    [BsonId] public ObjectId MongoId { get; set; }
    public string Name { get; set; } = null!;
    public int Value { get; set; }
}