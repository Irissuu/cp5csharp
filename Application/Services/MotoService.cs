using ElysiaAPI.Application.DTOs.Request;
using ElysiaAPI.Application.DTOs.Response;
using ElysiaAPI.Domain.Entity;
using ElysiaAPI.Domain.Repositories;
using ElysiaAPI.Domain.ValueObjects;

namespace ElysiaAPI.Application.Services;

public class MotoService
{
    private readonly IMotoRepository _repo;
    public MotoService(IMotoRepository repo) => _repo = repo;

    public async Task<MotoResponse> CreateAsync(MotoRequest req, CancellationToken ct = default)
    {
        var placa = Placa.Create(req.Placa);
        if (await _repo.PlacaExistsAsync(placa, ct))
            throw new InvalidOperationException("Já existe uma moto com essa placa.");

        var moto = new Moto(placa, req.Marca, req.Modelo, req.Ano);
        await _repo.AddAsync(moto, ct);
        return new MotoResponse { Id = moto.Id, Placa = moto.Placa.Value, Marca = moto.Marca, Modelo = moto.Modelo, Ano = moto.Ano };
    }

    public async Task<MotoResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var m = await _repo.GetByIdAsync(id, ct);
        return m is null ? null : new MotoResponse { Id = m.Id, Placa = m.Placa.Value, Marca = m.Marca, Modelo = m.Modelo, Ano = m.Ano };
    }

    public async Task<List<MotoResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await _repo.ListAsync(ct);
        return list.Select(m => new MotoResponse { Id = m.Id, Placa = m.Placa.Value, Marca = m.Marca, Modelo = m.Modelo, Ano = m.Ano }).ToList();
    }

    public Task<bool> UpdateAsync(int id, MotoRequest req, CancellationToken ct = default)
        => _repo.UpdateAsync(id, new Moto(Placa.Create(req.Placa), req.Marca, req.Modelo, req.Ano), ct);

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);
}