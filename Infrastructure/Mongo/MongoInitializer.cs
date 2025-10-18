using MongoDB.Driver;

namespace ElysiaAPI.Infrastructure.Mongo;

public static class MongoInitializer
{
    public static async Task EnsureAsync(MongoDbContext ctx, CancellationToken ct = default)
    {
        var motoIdx = new CreateIndexModel<MotoDocument>(
            Builders<MotoDocument>.IndexKeys.Ascending(x => x.Placa),
            new CreateIndexOptions { Unique = true, Name = "ux_motos_placa" });
        await ctx.Motos.Indexes.CreateOneAsync(motoIdx, cancellationToken: ct);

        var vagaIdx = new CreateIndexModel<VagaDocument>(
            Builders<VagaDocument>.IndexKeys
                .Ascending(x => x.Patio)
                .Ascending(x => x.Numero),
            new CreateIndexOptions { Unique = true, Name = "ux_vagas_patio_numero" });
        await ctx.Vagas.Indexes.CreateOneAsync(vagaIdx, cancellationToken: ct);

        await EnsureCounterAsync(ctx, "motos", ct);
        await EnsureCounterAsync(ctx, "vagas", ct);
    }

    private static async Task EnsureCounterAsync(MongoDbContext ctx, string name, CancellationToken ct)
    {
        var exists = await ctx.Counters.Find(x => x.Name == name).AnyAsync(ct);
        if (!exists)
        {
            await ctx.Counters.InsertOneAsync(new CounterDocument { Name = name, Value = 0 }, cancellationToken: ct);
        }
    }
}