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
    [Route("api/v{version:apiVersion}/motos")]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _service;
        public MotoController(MotoService service) => _service = service;
        
        private static MotoResponse ToResponse(MotoResponse m) => m; 

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotoResponse>), 200)]
        public async Task<ActionResult<IEnumerable<MotoResponse>>> GetMotos()
        {
            var motos = await _service.ListAsync();
            return Ok(motos.Select(ToResponse));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MotoResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MotoResponse>> GetMoto(int id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto is null) return NotFound();
            return Ok(ToResponse(moto));
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MotoResponse>), 200)]
        public async Task<ActionResult<IEnumerable<MotoResponse>>> SearchMoto([FromQuery] string placa)
        {
            placa ??= string.Empty;
            var all = await _service.ListAsync();
            var filtered = all.Where(m => (m.Placa ?? string.Empty).Contains(placa, StringComparison.OrdinalIgnoreCase));
            return Ok(filtered.Select(ToResponse));
        }

        [HttpPost]
        [ProducesResponseType(typeof(MotoResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MotoResponse>> CreateMoto([FromBody] MotoRequest request)
        {
            try
            {
                var created = await _service.CreateAsync(request);
                var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
                return CreatedAtAction(nameof(GetMoto), new { id = created.Id, version }, created);
            }
            catch (ArgumentException ex)           { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex)   { return BadRequest(ex.Message); }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMoto(int id, [FromBody] MotoRequest request)
        {
            var ok = await _service.UpdateAsync(id, request);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
