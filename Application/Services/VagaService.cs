using ElysiaAPI.Application.DTOs.Request;
using ElysiaAPI.Application.DTOs.Response;
using ElysiaAPI.Domain.Entity;
using ElysiaAPI.Domain.Repositories;

namespace ElysiaAPI.Application.Services;

public class VagaService
{
    private readonly IVagaRepository _repo;
    public VagaService(IVagaRepository repo) => _repo = repo;

    public async Task<VagaResponse> CreateAsync(VagaRequest req, CancellationToken ct = default)
    {
        if (await _repo.ExistsPatioNumeroAsync(req.Patio, req.Numero, ct))
            throw new InvalidOperationException("Já existe uma vaga com esse número nesse pátio.");

        var vaga = new Vaga(req.Numero, req.Patio) { Status = string.IsNullOrWhiteSpace(req.Status) ? "Livre" : req.Status };
        await _repo.AddAsync(vaga, ct);
        return new VagaResponse { Id = vaga.Id, Numero = vaga.Numero, Patio = vaga.Patio, Status = vaga.Status };
    }

    public async Task<VagaResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var v = await _repo.GetByIdAsync(id, ct);
        return v is null ? null : new VagaResponse { Id = v.Id, Numero = v.Numero, Patio = v.Patio, Status = v.Status };
    }

    public async Task<List<VagaResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await _repo.ListAsync(ct);
        return list.Select(v => new VagaResponse { Id = v.Id, Numero = v.Numero, Patio = v.Patio, Status = v.Status }).ToList();
    }

    public Task<bool> UpdateAsync(int id, VagaRequest req, CancellationToken ct = default)
        => _repo.UpdateAsync(id, new Vaga(req.Numero, req.Patio) { Status = req.Status }, ct);

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);
}