namespace ProyectoGrupo1.API.DTOs
{
    public class LookupItemDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }

    public class ProveedorDto
    {
        public int Id { get; set; }
        public string NombreProveedor { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Telefono { get; set; } = "";
    }

    public class ProveedorSaveDto
    {
        public string NombreProveedor { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Telefono { get; set; } = "";
    }
}
