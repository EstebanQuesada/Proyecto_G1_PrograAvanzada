namespace ProyectoGrupo1.Api.DTOs.Admin
{
    public record PagedResult<T>(int Total, IEnumerable<T> Items);
}
