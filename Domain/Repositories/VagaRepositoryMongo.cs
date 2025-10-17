using ElysiaAPI.Domain.Entity;
using ElysiaAPI.Domain.Repositories;
using ElysiaAPI.Infrastructure.Mongo;
using MongoDB.Driver;

namespace ElysiaAPI.Infrastructure.Repositories;

public class VagaRepositoryMongo : IVagaRepository
{
    private readonly MongoDbContext _ctx;
    public VagaRepositoryMongo(MongoDbContext ctx) => _ctx = ctx;

    private async Task<int> NextIdAsync(string name, CancellationToken ct)
    {
        var filter = Builders<CounterDocument>.Filter.Eq(x => x.Name, name);
        var update = Builders<CounterDocument>.Update.Inc(x => x.Value, 1);
        var opts   = new FindOneAndUpdateOptions<CounterDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };
        var doc    = await _ctx.Counters.FindOneAndUpdateAsync(filter, update, opts, ct);
        return doc.Value;
    }

    public async Task AddAsync(Vaga vaga, CancellationToken ct = default)
    {
        var doc = new VagaDocument
        {
            Id     = await NextIdAsync("vagas", ct),
            Status = vaga.Status,
            Numero = vaga.Numero,
            Patio  = vaga.Patio
        };

        await _ctx.Vagas.InsertOneAsync(doc, cancellationToken: ct);

        // definir o Id na entidade de domínio
        vaga.DefinirId(doc.Id);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var res = await _ctx.Vagas.DeleteOneAsync(x => x.Id == id, ct);
        return res.DeletedCount > 0;
    }

    public async Task<bool> ExistsPatioNumeroAsync(string patio, int numero, CancellationToken ct = default)
        => await _ctx.Vagas.Find(v => v.Patio == patio && v.Numero == numero).AnyAsync(ct);

    public async Task<Vaga?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var d = await _ctx.Vagas.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        if (d is null) return null;

        var v = new Vaga(d.Numero, d.Patio) { Status = d.Status };
        v.DefinirId(d.Id);
        return v;
    }

    public async Task<IReadOnlyList<Vaga>> ListAsync(CancellationToken ct = default)
    {
        var docs = await _ctx.Vagas.Find(_ => true).ToListAsync(ct);

        var list = new List<Vaga>();
        foreach (var d in docs)
        {
            var v = new Vaga(d.Numero, d.Patio) { Status = d.Status };
            v.DefinirId(d.Id);
            list.Add(v);
        }
        return list;
    }

    public async Task<bool> UpdateAsync(int id, Vaga vaga, CancellationToken ct = default)
    {
        var update = Builders<VagaDocument>.Update
            .Set(x => x.Status, vaga.Status)
            .Set(x => x.Numero, vaga.Numero)
            .Set(x => x.Patio,  vaga.Patio);

        var res = await _ctx.Vagas.UpdateOneAsync(x => x.Id == id, update, cancellationToken: ct);
        return res.MatchedCount > 0;
    }
}
