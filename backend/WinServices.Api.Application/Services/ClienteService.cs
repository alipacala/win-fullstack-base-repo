using WinServices.Api.Application.Common;
using WinServices.Api.Application.DTOs;
using WinServices.Api.Application.Interfaces;

namespace WinServices.Api.Application.Services;

public sealed class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository) => _repository = repository;

    public async Task<Result<IEnumerable<ClienteDto>>> BuscarAsync(string termino, int limite, CancellationToken ct = default)
    {
        termino = termino?.Trim() ?? string.Empty;
        if (termino.Length < 3)
            return Result<IEnumerable<ClienteDto>>.Failure("El término de búsqueda debe tener al menos 3 caracteres.");

        var items = await _repository.BuscarAsync(termino, limite, ct);
        return Result<IEnumerable<ClienteDto>>.Success(items);
    }
}
