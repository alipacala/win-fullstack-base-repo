using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WinServices.Api.Application.Interfaces;

namespace WinServices.Api.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/clientes")]
[ApiVersion(1.0)]
public sealed class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service) => _service = service;

    /// <summary>GET /api/v1/clientes/buscar?q=abc&amp;limit=10</summary>
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(
        [FromQuery] string? q,
        [FromQuery] int limit = 10,
        CancellationToken ct = default)
    {
        var result = await _service.BuscarAsync(q ?? string.Empty, limit, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}
