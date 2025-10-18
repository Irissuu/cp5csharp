using Asp.Versioning;
using ElysiaAPI.Application.DTOs.Request;
using ElysiaAPI.Application.DTOs.Response;
using ElysiaAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElysiaAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/vagas")]
    public class VagaController : ControllerBase
    {
        private readonly VagaService _service;
        public VagaController(VagaService service) => _service = service;
        
        private static VagaResponse ToResponse(VagaResponse v) => v; 

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VagaResponse>), 200)]
        public async Task<ActionResult<IEnumerable<VagaResponse>>> GetVagas()
        {
            var vagas = await _service.ListAsync();
            return Ok(vagas.Select(ToResponse));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VagaResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<VagaResponse>> GetVaga(int id)
        {
            var vaga = await _service.GetByIdAsync(id);
            if (vaga is null) return NotFound();
            return Ok(ToResponse(vaga));
        }

        [HttpGet("patio")]
        [ProducesResponseType(typeof(IEnumerable<VagaResponse>), 200)]
        public async Task<ActionResult<IEnumerable<VagaResponse>>> GetVagasByPatio([FromQuery] string patio)
        {
            var p = (patio ?? string.Empty).Trim();
            var vagas = await _service.ListAsync();
            var filtered = vagas.Where(v => string.Equals(v.Patio, p, StringComparison.OrdinalIgnoreCase));
            return Ok(filtered.Select(ToResponse));
        }

        [HttpPost]
        [ProducesResponseType(typeof(VagaResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<VagaResponse>> CreateVaga([FromBody] VagaRequest request)
        {
            try
            {
                var created = await _service.CreateAsync(request);
                var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
                return CreatedAtAction(nameof(GetVaga), new { id = created.Id, version }, created);
            }
            catch (ArgumentException ex)           { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex)   { return Conflict(ex.Message); }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> UpdateVaga(int id, [FromBody] VagaRequest request)
        {
            var ok = await _service.UpdateAsync(id, request);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVaga(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
