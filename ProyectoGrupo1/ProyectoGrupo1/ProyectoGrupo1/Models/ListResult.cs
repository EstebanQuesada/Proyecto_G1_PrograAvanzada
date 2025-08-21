namespace ProyectoGrupo1.Models;

public class ListResult<T>
{
    public int Total { get; set; }
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
}

