namespace WinServices.Api.Domain.Entities;

// TODO: Reemplaza esta entidad de ejemplo con tu entidad de dominio real.
// El ejemplo usa almacenamiento en memoria (sin BD), por lo que no depende de Dapper.
public sealed class Cliente : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public static Cliente Create(string name, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Cliente { Name = name, Description = description };
    }

    public void Update(string name, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
