using System.Data;
using Dapper;
using ProyectoGrupo1.API.DTOs.ProductAdmin;
using ProyectoGrupo1.API.Infra;

namespace ProyectoGrupo1.API.Repositories
{
    public class AdminProductoRepository : IAdminProductoRepository
    {
        private readonly IDbConnectionFactory _factory;
        public AdminProductoRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<(int Total, IEnumerable<AdminProductoListItemDto> Items)> ListarAsync(
            int page, int pageSize, string? search)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@Page", page);
            p.Add("@PageSize", pageSize);
            p.Add("@Search", search);

            using var multi = await cn.QueryMultipleAsync(
                "dbo.usp_Admin_Producto_Listar", p, commandType: CommandType.StoredProcedure);

            var total = await multi.ReadFirstAsync<int>();
            var items = await multi.ReadAsync<AdminProductoListItemDto>();
            return (total, items);
        }

        public async Task<(AdminProductoDto? Producto, AdminLookupsDto Lookups)> ObtenerAsync(int id)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();
            p.Add("@ProductoID", id);

            using var multi = await cn.QueryMultipleAsync(
                "dbo.usp_Admin_Producto_Obtener", p, commandType: CommandType.StoredProcedure);

            var prod = await multi.ReadFirstOrDefaultAsync<AdminProductoDto>();

            var imagenes = (await multi.ReadAsync<string>()).ToList();
            var ptcs = (await multi.ReadAsync<AdminPtcDto>()).ToList();

            var lookups = new AdminLookupsDto
            {
                Categorias = (await multi.ReadAsync<LookupItem>()).ToList(),
                Marcas = (await multi.ReadAsync<LookupItem>()).ToList(),
                Proveedores = (await multi.ReadAsync<LookupItem>()).ToList(),
                Tallas = (await multi.ReadAsync<LookupItem>()).ToList(),
                Colores = (await multi.ReadAsync<LookupItem>()).ToList(),
            };

            if (prod is not null)
            {
                prod.Imagenes = imagenes;
                prod.PTCs = ptcs;
            }

            return (prod, lookups);
        }

        public async Task<int> CrearAsync(AdminProductoSaveDto dto)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();

            p.Add("@Nombre", dto.Nombre);
            p.Add("@Descripcion", dto.Descripcion);
            p.Add("@Precio", dto.Precio);
            p.Add("@CategoriaID", dto.CategoriaID);
            p.Add("@MarcaID", dto.MarcaID);
            p.Add("@ProveedorID", dto.ProveedorID);

            var dtImg = new DataTable();
            dtImg.Columns.Add("UrlImagen", typeof(string));
            foreach (var url in dto.Imagenes ?? Enumerable.Empty<string>())
                dtImg.Rows.Add(url);
            p.Add("@Imagenes", dtImg.AsTableValuedParameter("dbo.ImagenProductoType"));

            var dtPtc = new DataTable();
            dtPtc.Columns.Add("TallaID", typeof(int));
            dtPtc.Columns.Add("ColorID", typeof(int));
            dtPtc.Columns.Add("Stock", typeof(int));
            foreach (var x in dto.PTCs ?? Enumerable.Empty<AdminPtcDto>())
                dtPtc.Rows.Add(x.TallaID, x.ColorID, x.Stock);
            p.Add("@PTCs", dtPtc.AsTableValuedParameter("dbo.PTCType"));

            p.Add("@NuevoProductoID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await cn.ExecuteAsync("dbo.usp_Admin_Producto_Crear", p, commandType: CommandType.StoredProcedure);
            return p.Get<int>("@NuevoProductoID");
        }

        public async Task<bool> ActualizarAsync(int id, AdminProductoSaveDto dto)
        {
            using var cn = _factory.Create();
            var p = new DynamicParameters();

            p.Add("@ProductoID", id);
            p.Add("@Nombre", dto.Nombre);
            p.Add("@Descripcion", dto.Descripcion);
            p.Add("@Precio", dto.Precio);
            p.Add("@CategoriaID", dto.CategoriaID);
            p.Add("@MarcaID", dto.MarcaID);
            p.Add("@ProveedorID", dto.ProveedorID);

            var dtImg = new DataTable();
            dtImg.Columns.Add("UrlImagen", typeof(string));
            foreach (var url in dto.Imagenes ?? Enumerable.Empty<string>())
                dtImg.Rows.Add(url);
            p.Add("@Imagenes", dtImg.AsTableValuedParameter("dbo.ImagenProductoType"));

            var dtPtc = new DataTable();
            dtPtc.Columns.Add("TallaID", typeof(int));
            dtPtc.Columns.Add("ColorID", typeof(int));
            dtPtc.Columns.Add("Stock", typeof(int));
            foreach (var x in dto.PTCs ?? Enumerable.Empty<AdminPtcDto>())
                dtPtc.Rows.Add(x.TallaID, x.ColorID, x.Stock);
            p.Add("@PTCs", dtPtc.AsTableValuedParameter("dbo.PTCType"));

            var rows = await cn.ExecuteAsync("dbo.usp_Admin_Producto_Actualizar", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using var cn = _factory.Create();
            var rows = await cn.ExecuteAsync(
                "dbo.usp_Admin_Producto_Eliminar",
                new { ProductoID = id },
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
