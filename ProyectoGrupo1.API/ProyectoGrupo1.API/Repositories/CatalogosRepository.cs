using System.Data;
using Dapper;
using ProyectoGrupo1.API.DTOs;
using ProyectoGrupo1.API.Infra;

namespace ProyectoGrupo1.API.Repositories
{
    public class CatalogosRepository : ICatalogosRepository
    {
        private readonly IDbConnectionFactory _factory;
        public CatalogosRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<LookupItemDto>> ListarCategoriasAsync()
        {
            using var cn = _factory.Create();
            return await cn.QueryAsync<LookupItemDto>("dbo.usp_Admin_Categoria_Listar",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<int> CrearCategoriaAsync(string nombre)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Nombre", nombre);
            p.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await cn.ExecuteAsync("dbo.usp_Admin_Categoria_Crear", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NuevoId");
        }
        public async Task<bool> ActualizarCategoriaAsync(int id, string nombre)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Categoria_Actualizar",
                new { Id = id, Nombre = nombre }, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Categoria_Eliminar",
                new { Id = id }, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<IEnumerable<LookupItemDto>> ListarMarcasAsync()
        {
            using var cn = _factory.Create();
            return await cn.QueryAsync<LookupItemDto>("dbo.usp_Admin_Marca_Listar", commandType: CommandType.StoredProcedure);
        }
        public async Task<int> CrearMarcaAsync(string nombre)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Nombre", nombre);
            p.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await cn.ExecuteAsync("dbo.usp_Admin_Marca_Crear", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NuevoId");
        }
        public async Task<bool> ActualizarMarcaAsync(int id, string nombre)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Marca_Actualizar", new { Id = id, Nombre = nombre },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
        public async Task<bool> EliminarMarcaAsync(int id)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Marca_Eliminar", new { Id = id },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<IEnumerable<LookupItemDto>> ListarTallasAsync()
        {
            using var cn = _factory.Create();
            return await cn.QueryAsync<LookupItemDto>("dbo.usp_Admin_Talla_Listar", commandType: CommandType.StoredProcedure);
        }
        public async Task<int> CrearTallaAsync(string nombre)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Nombre", nombre);
            p.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await cn.ExecuteAsync("dbo.usp_Admin_Talla_Crear", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NuevoId");
        }
        public async Task<bool> ActualizarTallaAsync(int id, string nombre)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Talla_Actualizar", new { Id = id, Nombre = nombre },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
        public async Task<bool> EliminarTallaAsync(int id)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Talla_Eliminar", new { Id = id },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<IEnumerable<LookupItemDto>> ListarColoresAsync()
        {
            using var cn = _factory.Create();
            return await cn.QueryAsync<LookupItemDto>("dbo.usp_Admin_Color_Listar", commandType: CommandType.StoredProcedure);
        }
        public async Task<int> CrearColorAsync(string nombre)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Nombre", nombre);
            p.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await cn.ExecuteAsync("dbo.usp_Admin_Color_Crear", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NuevoId");
        }
        public async Task<bool> ActualizarColorAsync(int id, string nombre)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Color_Actualizar", new { Id = id, Nombre = nombre },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
        public async Task<bool> EliminarColorAsync(int id)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Color_Eliminar", new { Id = id },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<IEnumerable<ProveedorDto>> ListarProveedoresAsync()
        {
            using var cn = _factory.Create();
            return await cn.QueryAsync<ProveedorDto>("dbo.usp_Admin_Proveedor_Listar", commandType: CommandType.StoredProcedure);
        }
        public async Task<int> CrearProveedorAsync(ProveedorSaveDto dto)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Nombre", dto.NombreProveedor);
            p.Add("@Correo", dto.Correo);
            p.Add("@Telefono", dto.Telefono);
            p.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await cn.ExecuteAsync("dbo.usp_Admin_Proveedor_Crear", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NuevoId");
        }
        public async Task<bool> ActualizarProveedorAsync(int id, ProveedorSaveDto dto)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Proveedor_Actualizar",
                new { Id = id, Nombre = dto.NombreProveedor, Correo = dto.Correo, Telefono = dto.Telefono },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
        public async Task<bool> EliminarProveedorAsync(int id)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Proveedor_Eliminar", new { Id = id },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
