using ElysiaAPI.Domain.Entity;
using ElysiaAPI.Domain.Repositories;
using ElysiaAPI.Domain.ValueObjects;
using ElysiaAPI.Infrastructure.Mongo;
using MongoDB.Driver;

namespace ElysiaAPI.Infrastructure.Repositories;

public class MotoRepositoryMongo : IMotoRepository
{
    private readonly MongoDbContext _ctx;
    public MotoRepositoryMongo(MongoDbContext ctx) => _ctx = ctx;

    private async Task<int> NextIdAsync(string name, CancellationToken ct)
    {
        var filter = Builders<CounterDocument>.Filter.Eq(x => x.Name, name);
        var update = Builders<CounterDocument>.Update.Inc(x => x.Value, 1);
        var opts   = new FindOneAndUpdateOptions<CounterDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };
        var doc    = await _ctx.Counters.FindOneAndUpdateAsync(filter, update, opts, ct);
        return doc.Value;
    }

    public async Task AddAsync(Moto moto, CancellationToken ct = default)
    {
        var doc = new MotoDocument
        {
            Id    = await NextIdAsync("motos", ct),
            Placa = moto.Placa.Value,
            Marca = moto.Marca,
            Modelo= moto.Modelo,
            Ano   = moto.Ano
        };

        await _ctx.Motos.InsertOneAsync(doc, cancellationToken: ct);

        moto.DefinirId(doc.Id);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var res = await _ctx.Motos.DeleteOneAsync(x => x.Id == id, ct);
        return res.DeletedCount > 0;
    }

    public async Task<Moto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var d = await _ctx.Motos.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        if (d is null) return null;

        var moto = new Moto(Placa.Create(d.Placa), d.Marca, d.Modelo, d.Ano);
        moto.DefinirId(d.Id);
        return moto;
    }

    public async Task<IReadOnlyList<Moto>> ListAsync(CancellationToken ct = default)
    {
        var docs = await _ctx.Motos.Find(_ => true).ToListAsync(ct);

        var list = new List<Moto>();
        foreach (var d in docs)
        {
            var m = new Moto(Placa.Create(d.Placa), d.Marca, d.Modelo, d.Ano);
            m.DefinirId(d.Id);
            list.Add(m);
        }
        return list;
    }

    public async Task<bool> PlacaExistsAsync(Placa placa, CancellationToken ct = default)
        => await _ctx.Motos.Find(x => x.Placa == placa.Value).AnyAsync(ct);

    public async Task<bool> UpdateAsync(int id, Moto moto, CancellationToken ct = default)
    {
        var update = Builders<MotoDocument>.Update
            .Set(x => x.Placa, moto.Placa.Value)
            .Set(x => x.Marca, moto.Marca)
            .Set(x => x.Modelo, moto.Modelo)
            .Set(x => x.Ano,   moto.Ano);

        var res = await _ctx.Motos.UpdateOneAsync(x => x.Id == id, update, cancellationToken: ct);
        return res.MatchedCount > 0;
    }
}
