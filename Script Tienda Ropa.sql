-- Crear la Base de Datos

CREATE DATABASE TiendaRopaDB;
GO

USE TiendaRopaDB;
GO

-- Tablas


CREATE TABLE Rol (
    RolID INT PRIMARY KEY IDENTITY,
    NombreRol NVARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    UsuarioID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Apellido NVARCHAR(100),
    Correo NVARCHAR(100) UNIQUE,
    Contrasena NVARCHAR(255),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    RolID INT,
    FOREIGN KEY (RolID) REFERENCES Rol(RolID)
);

CREATE TABLE DireccionUsuario (
    DireccionID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    Direccion NVARCHAR(255),
    Ciudad NVARCHAR(100),
    Provincia NVARCHAR(100),
    CodigoPostal NVARCHAR(20),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE CategoriaProducto (
    CategoriaID INT PRIMARY KEY IDENTITY,
    NombreCategoria NVARCHAR(100)
);

CREATE TABLE Marca (
    MarcaID INT PRIMARY KEY IDENTITY,
    NombreMarca NVARCHAR(100)
);

CREATE TABLE Proveedor (
    ProveedorID INT PRIMARY KEY IDENTITY,
    NombreProveedor NVARCHAR(100),
    Correo NVARCHAR(100),
    Telefono NVARCHAR(50)
);

CREATE TABLE Producto (
    ProductoID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Descripcion NVARCHAR(500),
    Precio DECIMAL(10,2),
    CategoriaID INT,
    MarcaID INT,
    ProveedorID INT,
    FOREIGN KEY (CategoriaID) REFERENCES CategoriaProducto(CategoriaID),
    FOREIGN KEY (MarcaID) REFERENCES Marca(MarcaID),
    FOREIGN KEY (ProveedorID) REFERENCES Proveedor(ProveedorID)
);

CREATE TABLE ImagenProducto (
    ImagenID INT PRIMARY KEY IDENTITY,
    ProductoID INT,
    UrlImagen NVARCHAR(255),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID)
);

CREATE TABLE Talla (
    TallaID INT PRIMARY KEY IDENTITY,
    NombreTalla NVARCHAR(10)
);

CREATE TABLE Color (
    ColorID INT PRIMARY KEY IDENTITY,
    NombreColor NVARCHAR(50)
);

CREATE TABLE ProductoTallaColor (
    PTCID INT PRIMARY KEY IDENTITY,
    ProductoID INT,
    TallaID INT,
    ColorID INT,
    Stock INT,
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    FOREIGN KEY (TallaID) REFERENCES Talla(TallaID),
    FOREIGN KEY (ColorID) REFERENCES Color(ColorID)
);

CREATE TABLE Carrito (
    CarritoID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE DetalleCarrito (
    DetalleID INT PRIMARY KEY IDENTITY,
    CarritoID INT,
    PTCID INT,
    Cantidad INT,
    FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID),
    FOREIGN KEY (PTCID) REFERENCES ProductoTallaColor(PTCID)
);

CREATE TABLE EstadoPedido (
    EstadoID INT PRIMARY KEY IDENTITY,
    NombreEstado NVARCHAR(50)
);

CREATE TABLE Pedido (
    PedidoID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    FechaPedido DATETIME DEFAULT GETDATE(),
    EstadoID INT,
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
    FOREIGN KEY (EstadoID) REFERENCES EstadoPedido(EstadoID)
);

CREATE TABLE DetallePedido (
    DetallePedidoID INT PRIMARY KEY IDENTITY,
    PedidoID INT,
    PTCID INT,
    Cantidad INT,
    PrecioUnitario DECIMAL(10,2),
    FOREIGN KEY (PedidoID) REFERENCES Pedido(PedidoID),
    FOREIGN KEY (PTCID) REFERENCES ProductoTallaColor(PTCID)
);

CREATE TABLE Pago (
    PagoID INT PRIMARY KEY IDENTITY,
    PedidoID INT,
    Monto DECIMAL(10,2),
    MetodoPago NVARCHAR(50),
    Estado NVARCHAR(50),
    FechaPago DATETIME,
    FOREIGN KEY (PedidoID) REFERENCES Pedido(PedidoID)
);

CREATE TABLE Envio (
    EnvioID INT PRIMARY KEY IDENTITY,
    PedidoID INT,
    Transportista NVARCHAR(100),
    NumeroSeguimiento NVARCHAR(100),
    CostoEnvio DECIMAL(10,2),
    FechaEnvio DATETIME,
    FOREIGN KEY (PedidoID) REFERENCES Pedido(PedidoID)
);

CREATE TABLE Promocion (
    PromocionID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    DescuentoPorcentaje DECIMAL(5,2),
    FechaInicio DATE,
    FechaFin DATE
);

CREATE TABLE ProductoPromocionado (
    ID INT PRIMARY KEY IDENTITY,
    ProductoID INT,
    PromocionID INT,
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    FOREIGN KEY (PromocionID) REFERENCES Promocion(PromocionID)
);

CREATE TABLE CalendarioEvento (
    EventoID INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(100),
    FechaEvento DATE,
    Descripcion NVARCHAR(255)
);

CREATE TABLE MensajeSistema (
    MensajeID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    Contenido NVARCHAR(255),
    FechaMensaje DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE CorreoEnviado (
    CorreoID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    Asunto NVARCHAR(100),
    Contenido NVARCHAR(255),
    FechaEnvio DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE Reseña (
    ReseñaID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    ProductoID INT,
    Calificacion INT,
    Comentario NVARCHAR(255),
    FechaReseña DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID)
);

CREATE TABLE ListaDeseos (
    ListaID INT PRIMARY KEY IDENTITY,
    UsuarioID INT,
    NombreLista NVARCHAR(100),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE DetalleListaDeseos (
    ID INT PRIMARY KEY IDENTITY,
    ListaID INT,
    ProductoID INT,
    FOREIGN KEY (ListaID) REFERENCES ListaDeseos(ListaID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID)
);

CREATE TABLE BitacoraErrores (
    ErrorID INT PRIMARY KEY IDENTITY,
    UsuarioID INT NULL,
    MensajeError NVARCHAR(500),
    FechaError DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE TextosGenerales (
    Clave NVARCHAR(100) PRIMARY KEY,
    Valor NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Contactos (
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Mensaje NVARCHAR(MAX) NOT NULL,
    FechaEnvio DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE PROCEDURE ObtenerDescripcionContacto
AS
BEGIN
    SELECT Valor FROM TextosGenerales WHERE Clave = 'DescripcionContacto'
END

INSERT INTO TextosGenerales (Clave, Valor)
VALUES ('DescripcionContacto', 'Aquí podrán enviar consultas, sugerencias o reclamos. Estamos para ayudarte en lo que necesites.');


INSERT INTO Rol (NombreRol) VALUES ('Cliente');
INSERT INTO Rol (NombreRol) VALUES ('Administrador');



SELECT * FROM Usuario WHERE Correo = 'hola@gmail.com';
USE TiendaRopaDB;
GO

SELECT 
    u.UsuarioID,
    u.Nombre,
    u.Apellido,
    u.Correo,
    d.Direccion,
    d.Ciudad,
    d.Provincia,
    d.CodigoPostal
FROM Usuario u
INNER JOIN DireccionUsuario d ON u.UsuarioID = d.UsuarioID
WHERE u.UsuarioID = 11;

--inserts de categorias
INSERT INTO CategoriaProducto (NombreCategoria) VALUES ('Camisas'), ('Pantalones'), ('Zapatos');

INSERT INTO Marca (NombreMarca) VALUES ('Nike'), ('Adidas'), ('Zara');

INSERT INTO Proveedor (NombreProveedor, Correo, Telefono)
VALUES ('Proveedor A', 'proveedorA@mail.com', '1111-2222');

--inserts de productos
INSERT INTO Producto (Nombre, Descripcion, Precio, CategoriaID, MarcaID, ProveedorID)
VALUES 
('Camisa Blanca', 'Camisa blanca de algodón, ideal para clima cálido.', 12900, 1, 3, 1),
('Pantalón Casual', 'Pantalón cómodo de mezclilla para uso diario.', 19900, 2, 2, 1),
('Zapatos Deportivos', 'Zapatos livianos y cómodos para correr.', 34900, 3, 1, 1);

INSERT INTO Producto (Nombre, Descripcion, Precio, CategoriaID, MarcaID, ProveedorID)
VALUES 
('Camisa Negra', 'Camisa de Mujer negra de algodón', 25000, 1, 1, 1),
('Camisa Adidas','Camisa blanco con verde', 22000, 1, 2, 1),
('Pantalon Mujer', 'Pantalon negro casual', 30000, 2,1, 1),
('Pantalon Zara Hombre','Pantalon de mezclilla', 32000, 2, 3,1);


--insert de imagenes 
INSERT INTO ImagenProducto (ProductoID, UrlImagen) 
VALUES 
(1, 'https://static.zara.net/assets/public/575b/1329/64dd4d359b7e/7d1416255c09/07446431250-p/07446431250-p.jpg?ts=1738314991683&w={width}'),
(2, 'https://www.hola.com/horizon/original_aspect_ratio/bbb788e9ffd9-chaleco-a.jpg'),
(3, 'https://img.pacifiko.com/PROD/resize/1/500x500/Y2FkY2UxYj.jpg');

INSERT INTO ImagenProducto (ProductoID, UrlImagen) 
VALUES 
(4, 'https://dpjye2wk9gi5z.cloudfront.net/wcsstore/ExtendedSitesCatalogAssetStore/images/catalog/zoom/3020471-0002V1.jpg'),
(5, 'https://resize.sprintercdn.com/b/1440x2160/products/0394116/adidas-collegiate_0394116_00_5_1322578255.jpg?w=1440&q=75'),
(6, 'https://woker.vtexassets.com/arquivos/ids/524375-800-800?v=638672829049170000&width=800&height=800&aspect=true'),
(7, 'https://static.zara.net/assets/public/4529/0343/9ec149a0b17f/a391362a4d7e/08062410406-p/08062410406-p.jpg?ts=1741276877704&w=560&f=auto');

---inserts de tallas
INSERT INTO Talla (NombreTalla) VALUES ('S'), ('M'), ('L'),('40');
INSERT INTO Color (NombreColor) VALUES ('Blanco'), ('Azul'), ('Negro');

---vinculacion producto con talla y color+ stock
-- Camisa Blanca
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(1, 1, 1, 10), -- S, Blanco
(1, 2, 1, 5);  -- M, Blanco

--camisa negra
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(4, 2, 3, 11),
(4, 3, 3, 15);

--camisa adidas blanco
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(5, 1, 1, 4),
(5, 2, 1, 8);

-- Pantalón
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(2, 2, 3, 7), -- M, negro
(2, 3, 3, 3); -- L, negro

--pantalon mujer negro
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(6, 1, 3, 15),
(6, 3, 3, 16);

---pantalon azul hombre
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(7, 2, 2, 20),
(7, 3, 2, 16);

-- Zapatos
INSERT INTO ProductoTallaColor (ProductoID, TallaID, ColorID, Stock)
VALUES 
(3, 4, 1, 8); -- 40, blanco

-- Insertar estados
INSERT INTO EstadoPedido (NombreEstado) VALUES 
('Pendiente'),
('Procesando'),
('Enviado'),
('Entregado'),
('Cancelado'),
('Devuelto');


CREATE OR ALTER PROCEDURE spObtenerHistorialPedidos
    @UsuarioID INT
AS
BEGIN
    SELECT 
        p.PedidoID,
        p.FechaPedido,
        ep.NombreEstado,
        dp.DetallePedidoID,
        ptc.PTCID,
        prod.Nombre AS Producto,
        ptc.Stock,
        c.NombreColor,
        t.NombreTalla,
        dp.Cantidad,
        dp.PrecioUnitario,
        ip.UrlImagen
    FROM Pedido p
    INNER JOIN EstadoPedido ep ON p.EstadoID = ep.EstadoID
    INNER JOIN DetallePedido dp ON p.PedidoID = dp.PedidoID
    INNER JOIN ProductoTallaColor ptc ON dp.PTCID = ptc.PTCID
    INNER JOIN Producto prod ON ptc.ProductoID = prod.ProductoID
    INNER JOIN Color c ON ptc.ColorID = c.ColorID
    INNER JOIN Talla t ON ptc.TallaID = t.TallaID
    LEFT JOIN ImagenProducto ip ON prod.ProductoID = ip.ProductoID
        AND ip.ImagenID = (
            SELECT MIN(ImagenID) FROM ImagenProducto WHERE ProductoID = prod.ProductoID
        )
    WHERE p.UsuarioID = @UsuarioID
    ORDER BY p.FechaPedido DESC, p.PedidoID DESC
END





--Procedimientos Almacenados Usuarios

--Validar login
CREATE OR ALTER PROCEDURE sp_Usuario_Validar
    @Correo NVARCHAR(256),
    @ContrasenaHash NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 UsuarioID, Nombre, Correo
    FROM Usuario
    WHERE Correo = @Correo AND Contrasena = @ContrasenaHash;
END
GO

--Registro usuario + dirección (transacción)
CREATE OR ALTER PROCEDURE sp_Usuario_Registrar
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Correo NVARCHAR(256),
    @ContrasenaHash NVARCHAR(256),
    @Direccion NVARCHAR(200),
    @Ciudad NVARCHAR(100),
    @Provincia NVARCHAR(100),
    @CodigoPostal NVARCHAR(20),
    @NuevoUsuarioID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        INSERT INTO Usuario (Nombre, Apellido, Correo, Contrasena, FechaRegistro, RolID)
        VALUES (@Nombre, @Apellido, @Correo, @ContrasenaHash, GETDATE(), 1);

        SET @NuevoUsuarioID = SCOPE_IDENTITY();

        INSERT INTO DireccionUsuario (UsuarioID, Direccion, Ciudad, Provincia, CodigoPostal)
        VALUES (@NuevoUsuarioID, @Direccion, @Ciudad, @Provincia, @CodigoPostal);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

--Perfil completo
CREATE OR ALTER PROCEDURE sp_Usuario_ObtenerPerfil
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UsuarioID, u.Nombre, u.Apellido, u.Correo, u.Contrasena, u.RolID,
           ISNULL(d.Direccion,'') AS Direccion, ISNULL(d.Ciudad,'') AS Ciudad,
           ISNULL(d.Provincia,'') AS Provincia, ISNULL(d.CodigoPostal,'') AS CodigoPostal
    FROM Usuario u
    LEFT JOIN DireccionUsuario d ON d.UsuarioID = u.UsuarioID
    WHERE u.UsuarioID = @UsuarioID;
END
GO

--Actualizar perfil y dirección (UPSERT dirección)
CREATE OR ALTER PROCEDURE sp_Usuario_ActualizarPerfilYDireccion
    @UsuarioID INT,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Correo NVARCHAR(256),
    @Direccion NVARCHAR(200),
    @Ciudad NVARCHAR(100),
    @Provincia NVARCHAR(100),
    @CodigoPostal NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Usuario
        SET Nombre = @Nombre, Apellido = @Apellido, Correo = @Correo
        WHERE UsuarioID = @UsuarioID;

        IF EXISTS (SELECT 1 FROM DireccionUsuario WHERE UsuarioID = @UsuarioID)
            UPDATE DireccionUsuario
            SET Direccion=@Direccion, Ciudad=@Ciudad, Provincia=@Provincia, CodigoPostal=@CodigoPostal
            WHERE UsuarioID=@UsuarioID;
        ELSE
            INSERT INTO DireccionUsuario (UsuarioID, Direccion, Ciudad, Provincia, CodigoPostal)
            VALUES (@UsuarioID, @Direccion, @Ciudad, @Provincia, @CodigoPostal);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

--Obtener hash actual de contraseña
CREATE OR ALTER PROCEDURE sp_Usuario_ObtenerHashContrasena
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Contrasena FROM Usuario WHERE UsuarioID = @UsuarioID;
END
GO

--Cambiar contraseña
CREATE OR ALTER PROCEDURE sp_Usuario_CambiarContrasena
    @UsuarioID INT,
    @NuevaContrasenaHash NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario SET Contrasena = @NuevaContrasenaHash WHERE UsuarioID = @UsuarioID;
END
GO


--Procedimientos almacenados Admin

IF COL_LENGTH('dbo.Usuario','Bloqueado') IS NULL
    ALTER TABLE dbo.Usuario ADD Bloqueado BIT NOT NULL CONSTRAINT DF_Usuario_Bloqueado DEFAULT (0);
IF COL_LENGTH('dbo.Usuario','Activo') IS NULL
    ALTER TABLE dbo.Usuario ADD Activo BIT NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT (1);

--Listar
CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Listar
    @Page     INT = 1,
    @PageSize INT = 10,
    @Search   NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @Page     = CASE WHEN @Page IS NULL OR @Page < 1 THEN 1  ELSE @Page     END;
    SET @PageSize = CASE WHEN @PageSize IS NULL OR @PageSize < 1 THEN 10 ELSE @PageSize END;

    DECLARE @s NVARCHAR(202) =
        CASE WHEN @Search IS NULL OR LTRIM(RTRIM(@Search)) = '' THEN NULL
             ELSE '%' + LTRIM(RTRIM(@Search)) + '%' END;

    ;WITH q AS (
        SELECT u.UsuarioID, u.Nombre, u.Apellido, u.Correo, u.RolID, u.FechaRegistro,
               ISNULL(u.Bloqueado,0) AS Bloqueado, ISNULL(u.Activo,1) AS Activo
        FROM Usuario u
        WHERE (@s IS NULL OR u.Nombre LIKE @s OR u.Apellido LIKE @s OR u.Correo LIKE @s)
    )
    SELECT COUNT(*) AS Total FROM q;

    ;WITH q AS (
        SELECT u.UsuarioID, u.Nombre, u.Apellido, u.Correo, u.RolID, u.FechaRegistro,
               ISNULL(u.Bloqueado,0) AS Bloqueado, ISNULL(u.Activo,1) AS Activo
        FROM Usuario u
        WHERE (@s IS NULL OR u.Nombre LIKE @s OR u.Apellido LIKE @s OR u.Correo LIKE @s)
    )
    SELECT UsuarioID, Nombre, Apellido, Correo, RolID, FechaRegistro, Bloqueado, Activo
    FROM q
    ORDER BY UsuarioID DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO


--Obtener detalle

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Obtener
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UsuarioID, u.Nombre, u.Apellido, u.Correo, u.RolID, u.FechaRegistro,
           ISNULL(u.Bloqueado,0) Bloqueado, ISNULL(u.Activo,1) Activo,
           ISNULL(d.Direccion,'') Direccion, ISNULL(d.Ciudad,'') Ciudad,
           ISNULL(d.Provincia,'') Provincia, ISNULL(d.CodigoPostal,'') CodigoPostal
    FROM Usuario u
    LEFT JOIN DireccionUsuario d ON d.UsuarioID = u.UsuarioID
    WHERE u.UsuarioID = @UsuarioID;
END
GO
-- Crear
CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Crear
 @Nombre nvarchar(100), @Apellido nvarchar(100), @Correo nvarchar(200), @ContrasenaHash nvarchar(200),
 @RolID int, @Direccion nvarchar(200), @Ciudad nvarchar(100), @Provincia nvarchar(100), @CodigoPostal nvarchar(20),
 @NuevoUsuarioID int OUTPUT
AS
BEGIN
    INSERT INTO Usuario(Nombre, Apellido, Correo, Contrasena, RolID, FechaRegistro, Activo, Bloqueado)
    VALUES(@Nombre,@Apellido,@Correo,@ContrasenaHash,@RolID,GETDATE(),1,0);

    SET @NuevoUsuarioID = SCOPE_IDENTITY();

    MERGE DireccionUsuario AS t
    USING (SELECT @NuevoUsuarioID AS UsuarioID, @Direccion AS Direccion, @Ciudad AS Ciudad, @Provincia AS Provincia, @CodigoPostal AS CodigoPostal) AS s
    ON t.UsuarioID = s.UsuarioID
    WHEN MATCHED THEN UPDATE SET Direccion=s.Direccion, Ciudad=s.Ciudad, Provincia=s.Provincia, CodigoPostal=s.CodigoPostal
    WHEN NOT MATCHED THEN INSERT(UsuarioID,Direccion,Ciudad,Provincia,CodigoPostal)
    VALUES(s.UsuarioID,s.Direccion,s.Ciudad,s.Provincia,s.CodigoPostal);
END
GO

-- Actualizar
CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Actualizar
 @UsuarioID int, @Nombre nvarchar(100), @Apellido nvarchar(100), @Correo nvarchar(200), @RolID int,
 @Direccion nvarchar(200), @Ciudad nvarchar(100), @Provincia nvarchar(100), @CodigoPostal nvarchar(20)
AS
BEGIN
  UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Correo=@Correo, RolID=@RolID WHERE UsuarioID=@UsuarioID;

  MERGE DireccionUsuario AS t
  USING (SELECT @UsuarioID AS UsuarioID, @Direccion AS Direccion, @Ciudad AS Ciudad, @Provincia AS Provincia, @CodigoPostal AS CodigoPostal) AS s
  ON t.UsuarioID = s.UsuarioID
  WHEN MATCHED THEN UPDATE SET Direccion=s.Direccion, Ciudad=s.Ciudad, Provincia=s.Provincia, CodigoPostal=s.CodigoPostal
  WHEN NOT MATCHED THEN INSERT(UsuarioID,Direccion,Ciudad,Provincia,CodigoPostal)
  VALUES(s.UsuarioID,s.Direccion,s.Ciudad,s.Provincia,s.CodigoPostal);
END
GO

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_CambiarRol @UsuarioID int,@RolID int AS
UPDATE Usuario SET RolID=@RolID WHERE UsuarioID=@UsuarioID;
GO

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Bloquear @UsuarioID int,@Bloqueado bit AS
UPDATE Usuario SET Bloqueado=@Bloqueado WHERE UsuarioID=@UsuarioID;
GO

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_ResetPassword @UsuarioID int,@NuevaContrasenaHash nvarchar(200) AS
UPDATE Usuario SET Contrasena=@NuevaContrasenaHash WHERE UsuarioID=@UsuarioID;
GO

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Eliminar @UsuarioID int AS
UPDATE Usuario SET Activo=0 WHERE UsuarioID=@UsuarioID;
GO

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Restaurar @UsuarioID int AS
UPDATE Usuario SET Activo=1 WHERE UsuarioID=@UsuarioID;
GO

CREATE OR ALTER PROCEDURE sp_Admin_Usuario_Obtener @UsuarioID int AS
BEGIN
 SELECT TOP 1 u.UsuarioID,u.Nombre,u.Apellido,u.Correo,u.RolID,u.Contrasena,
        ISNULL(d.Direccion,'') Direccion,ISNULL(d.Ciudad,'') Ciudad,ISNULL(d.Provincia,'') Provincia,ISNULL(d.CodigoPostal,'') CodigoPostal,
        ISNULL(u.Bloqueado,0) Bloqueado,ISNULL(u.Activo,1) Activo
 FROM Usuario u LEFT JOIN DireccionUsuario d ON d.UsuarioID=u.UsuarioID
 WHERE u.UsuarioID=@UsuarioID;
END
GO

-- Login valida credenciales y devuelve estado
CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Validar
    @Correo           nvarchar(200),
    @ContrasenaHash   nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        UsuarioID,
        Nombre,
        Apellido,
        Correo,
        RolID,
        ISNULL(Activo, 1)     AS Activo,
        ISNULL(Bloqueado, 0)  AS Bloqueado
    FROM dbo.Usuario
    WHERE Correo = @Correo
      AND Contrasena = @ContrasenaHash;
END
GO




--Catalogo


-- Catálogo con filtros
CREATE OR ALTER PROCEDURE dbo.usp_Producto_Catalogo
    @Busqueda   NVARCHAR(100) = NULL,
    @Categoria  NVARCHAR(100) = NULL,
    @PrecioMin  DECIMAL(18,2) = NULL,
    @PrecioMax  DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.ProductoID,
        p.Nombre,
        LEFT(p.Descripcion, 100) AS DescripcionCorta,
        p.Precio,
        c.NombreCategoria AS Categoria,
        (SELECT TOP (1) UrlImagen 
         FROM ImagenProducto 
         WHERE ProductoID = p.ProductoID 
         ORDER BY ImagenID) AS UrlImagenPrincipal
    FROM Producto p
    INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
    WHERE (@Busqueda IS NULL OR p.Nombre LIKE '%' + @Busqueda + '%')
      AND (@Categoria IS NULL OR c.NombreCategoria = @Categoria)
      AND (@PrecioMin IS NULL OR p.Precio >= @PrecioMin)
      AND (@PrecioMax IS NULL OR p.Precio <= @PrecioMax)
    ORDER BY p.Nombre;
END
GO

-- Paquete de detalle (múltiples result sets)
CREATE OR ALTER PROCEDURE dbo.usp_Producto_Detalle_Paquete
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) detalle
    SELECT p.ProductoID, p.Nombre, p.Descripcion, p.Precio, c.NombreCategoria AS Categoria
    FROM Producto p
    INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
    WHERE p.ProductoID = @ProductoID;

    -- 2) imágenes
    SELECT UrlImagen 
    FROM ImagenProducto 
    WHERE ProductoID = @ProductoID
    ORDER BY ImagenID;

    -- 3) tallas
    SELECT DISTINCT t.NombreTalla
    FROM ProductoTallaColor ptc
    INNER JOIN Talla t ON ptc.TallaID = t.TallaID
    WHERE ptc.ProductoID = @ProductoID;

    -- 4) colores
    SELECT DISTINCT c.NombreColor
    FROM ProductoTallaColor ptc
    INNER JOIN Color c ON ptc.ColorID = c.ColorID
    WHERE ptc.ProductoID = @ProductoID;

    -- 5) PTC
    SELECT ptc.PTCID, ptc.ProductoID, ptc.TallaID, ptc.ColorID, ptc.Stock, t.NombreTalla, c.NombreColor
    FROM ProductoTallaColor ptc
    INNER JOIN Talla t ON ptc.TallaID = t.TallaID
    INNER JOIN Color c ON ptc.ColorID = c.ColorID
    WHERE ptc.ProductoID = @ProductoID;
END
GO

-- Categorías
CREATE OR ALTER PROCEDURE dbo.usp_Producto_Categorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT NombreCategoria FROM CategoriaProducto ORDER BY NombreCategoria;
END
GO

CREATE TYPE dbo.DetallePedidoType AS TABLE(PTCID INT, Cantidad INT, PrecioUnitario DECIMAL(10,2) NULL);

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_Crear
  @UsuarioID INT, @Detalles dbo.DetallePedidoType READONLY, @NuevoPedidoID INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON; SET XACT_ABORT ON;
  BEGIN TRY
    BEGIN TRAN;
    IF EXISTS(
      SELECT 1 FROM @Detalles d
      JOIN ProductoTallaColor ptc WITH(UPDLOCK, ROWLOCK) ON ptc.PTCID=d.PTCID
      WHERE d.Cantidad > ptc.Stock
    ) RAISERROR('STOCK_INSUFICIENTE',16,1);

    INSERT INTO Pedido(UsuarioID, FechaPedido, EstadoID) VALUES(@UsuarioID, GETDATE(), 1);
    SET @NuevoPedidoID = SCOPE_IDENTITY();

    INSERT INTO DetallePedido(PedidoID, PTCID, Cantidad, PrecioUnitario)
    SELECT @NuevoPedidoID, d.PTCID, d.Cantidad, ISNULL(d.PrecioUnitario, p.Precio)
    FROM @Detalles d JOIN ProductoTallaColor ptc ON ptc.PTCID=d.PTCID
                     JOIN Producto p ON p.ProductoID=ptc.ProductoID;

    UPDATE ptc SET ptc.Stock = ptc.Stock - d.Cantidad
    FROM ProductoTallaColor ptc JOIN @Detalles d ON d.PTCID=ptc.PTCID;
    COMMIT;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK; THROW;
  END CATCH
END

-- Obtener PTC por Id
CREATE OR ALTER PROCEDURE dbo.usp_Producto_PTC_PorId
    @PTCID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ptc.PTCID, ptc.ProductoID, ptc.TallaID, ptc.ColorID, ptc.Stock,
           t.NombreTalla, c.NombreColor
    FROM ProductoTallaColor ptc
    INNER JOIN Talla  t ON t.TallaID  = ptc.TallaID
    INNER JOIN Color  c ON c.ColorID  = ptc.ColorID
    WHERE ptc.PTCID = @PTCID;
END
GO



CREATE OR ALTER PROCEDURE dbo.usp_Producto_Catalogo_Paginado
 @Busqueda NVARCHAR(100)=NULL, @Categoria NVARCHAR(100)=NULL,
 @PrecioMin DECIMAL(18,2)=NULL, @PrecioMax DECIMAL(18,2)=NULL,
 @Page INT = 1, @PageSize INT = 20
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH Q AS (
    SELECT 
      p.ProductoID, p.Nombre,
      LEFT(p.Descripcion, 100) AS DescripcionCorta,
      p.Precio, c.NombreCategoria AS Categoria,
      (SELECT TOP (1) UrlImagen FROM ImagenProducto WHERE ProductoID = p.ProductoID ORDER BY ImagenID) AS UrlImagenPrincipal
    FROM Producto p
    INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
    WHERE (@Busqueda IS NULL OR p.Nombre LIKE '%' + @Busqueda + '%')
      AND (@Categoria IS NULL OR c.NombreCategoria = @Categoria)
      AND (@PrecioMin IS NULL OR p.Precio >= @PrecioMin)
      AND (@PrecioMax IS NULL OR p.Precio <= @PrecioMax)
  )
  SELECT * FROM Q
  ORDER BY Nombre
  OFFSET (@Page-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;

  SELECT COUNT(*) AS Total FROM Q;
END


CREATE OR ALTER PROCEDURE dbo.usp_Producto_PTC_PorId
    @PTCID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ptc.PTCID, ptc.ProductoID, ptc.TallaID, ptc.ColorID, ptc.Stock,
           t.NombreTalla, c.NombreColor
    FROM ProductoTallaColor ptc
    INNER JOIN Talla  t ON t.TallaID  = ptc.TallaID
    INNER JOIN Color  c ON c.ColorID  = ptc.ColorID
    WHERE ptc.PTCID = @PTCID;
END
GO

<<<<<<< Updated upstream
-- Tipos de tabla (TVP) para imágenes y PTC
IF TYPE_ID(N'dbo.ImagenProductoType') IS NULL
    CREATE TYPE dbo.ImagenProductoType AS TABLE(UrlImagen NVARCHAR(500) NOT NULL);
GO

IF TYPE_ID(N'dbo.PTCType') IS NULL
    CREATE TYPE dbo.PTCType AS TABLE(
        TallaID INT NOT NULL,
        ColorID INT NOT NULL,
        Stock   INT NOT NULL CHECK (Stock >= 0)
    );
GO

-- Listar con paginación y búsqueda
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Listar
    @Page     INT = 1,
    @PageSize INT = 10,
    @Search   NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @s NVARCHAR(202) =
        CASE WHEN @Search IS NULL OR LTRIM(RTRIM(@Search)) = ''
             THEN NULL
             ELSE '%' + LTRIM(RTRIM(@Search)) + '%'
        END;

    DECLARE @q TABLE (
        ProductoID         INT,
        Nombre             NVARCHAR(200),
        Precio             DECIMAL(18,2),
        CategoriaID        INT,
        Categoria          NVARCHAR(200),
        UrlImagenPrincipal NVARCHAR(500) NULL
    );

    INSERT INTO @q (ProductoID, Nombre, Precio, CategoriaID, Categoria, UrlImagenPrincipal)
    SELECT  p.ProductoID,
            p.Nombre,
            p.Precio,
            c.CategoriaID,
            c.NombreCategoria AS Categoria,          
            (SELECT TOP(1) UrlImagen
             FROM ImagenProducto i
             WHERE i.ProductoID = p.ProductoID
             ORDER BY i.ImagenID)
    FROM Producto p
    INNER JOIN CategoriaProducto c ON c.CategoriaID = p.CategoriaID
    WHERE (@s IS NULL
           OR p.Nombre       LIKE @s
           OR p.Descripcion  LIKE @s
           OR c.NombreCategoria LIKE @s);

    SELECT COUNT(*) AS Total
    FROM @q;

    SELECT  ProductoID, Nombre, Precio, CategoriaID, Categoria, UrlImagenPrincipal
    FROM @q
    ORDER BY ProductoID DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO


-- Obtener detalle para edición (producto + imágenes + PTC + lookups)
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Obtener
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
        p.ProductoID, p.Nombre, p.Descripcion, p.Precio,
        p.CategoriaID, p.MarcaID, p.ProveedorID
    FROM Producto p
    WHERE p.ProductoID = @ProductoID;

    SELECT UrlImagen
    FROM ImagenProducto
    WHERE ProductoID = @ProductoID
    ORDER BY ImagenID;

    SELECT ptc.PTCID, ptc.TallaID, t.NombreTalla, ptc.ColorID, c.NombreColor, ptc.Stock
    FROM ProductoTallaColor ptc
    INNER JOIN Talla t ON t.TallaID = ptc.TallaID
    INNER JOIN Color c ON c.ColorID = ptc.ColorID
    WHERE ptc.ProductoID = @ProductoID;

	-- Lookups para combos en el front (alias a Id/Nombre)
	SELECT CategoriaID  AS Id, NombreCategoria AS Nombre FROM CategoriaProducto ORDER BY NombreCategoria;
	SELECT MarcaID      AS Id, NombreMarca     AS Nombre FROM Marca            ORDER BY NombreMarca;
	SELECT ProveedorID  AS Id, NombreProveedor AS Nombre FROM Proveedor        ORDER BY NombreProveedor;
	SELECT TallaID      AS Id, NombreTalla     AS Nombre FROM Talla            ORDER BY NombreTalla;
	SELECT ColorID      AS Id, NombreColor     AS Nombre FROM Color            ORDER BY NombreColor;

END
GO

-- Crear producto con imágenes y PTC (TVPs)
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Crear
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @Precio DECIMAL(18,2),
    @CategoriaID INT,
    @MarcaID INT,
    @ProveedorID INT,
    @Imagenes dbo.ImagenProductoType READONLY,
    @PTCs dbo.PTCType READONLY,
    @NuevoProductoID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        INSERT INTO Producto (Nombre, Descripcion, Precio, CategoriaID, MarcaID, ProveedorID)
        VALUES (@Nombre, @Descripcion, @Precio, @CategoriaID, @MarcaID, @ProveedorID);

        SET @NuevoProductoID = SCOPE_IDENTITY();

        INSERT INTO ImagenProducto(ProductoID, UrlImagen)
        SELECT @NuevoProductoID, UrlImagen FROM @Imagenes;

        INSERT INTO ProductoTallaColor(ProductoID, TallaID, ColorID, Stock)
        SELECT @NuevoProductoID, TallaID, ColorID, Stock FROM @PTCs;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

-- Actualizar producto: reemplaza imágenes y PTC del producto
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Actualizar
    @ProductoID INT,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @Precio DECIMAL(18,2),
    @CategoriaID INT,
    @MarcaID INT,
    @ProveedorID INT,
    @Imagenes dbo.ImagenProductoType READONLY,
    @PTCs dbo.PTCType READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Producto
        SET Nombre=@Nombre, Descripcion=@Descripcion, Precio=@Precio,
            CategoriaID=@CategoriaID, MarcaID=@MarcaID, ProveedorID=@ProveedorID
        WHERE ProductoID=@ProductoID;

        DELETE FROM ImagenProducto WHERE ProductoID=@ProductoID;
        INSERT INTO ImagenProducto(ProductoID, UrlImagen)
        SELECT @ProductoID, UrlImagen FROM @Imagenes;

        DELETE FROM ProductoTallaColor WHERE ProductoID=@ProductoID;
        INSERT INTO ProductoTallaColor(ProductoID, TallaID, ColorID, Stock)
        SELECT @ProductoID, TallaID, ColorID, Stock FROM @PTCs;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

-- Eliminar producto (fuerte: elimina imágenes y PTC)
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Eliminar
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        DELETE FROM ImagenProducto WHERE ProductoID=@ProductoID;
        DELETE FROM ProductoTallaColor WHERE ProductoID=@ProductoID;
        DELETE FROM Producto WHERE ProductoID=@ProductoID;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO




--Contactos
CREATE PROCEDURE SP_GuardarContacto
    @Nombre NVARCHAR(100),
    @Correo NVARCHAR(100),
    @Mensaje NVARCHAR(MAX),
    @FechaEnvio DATETIME
AS
BEGIN
    INSERT INTO Contactos (Nombre, Correo, Mensaje, FechaEnvio)
    VALUES (@Nombre, @Correo, @Mensaje, @FechaEnvio);
END




--Contacto Admin
IF OBJECT_ID('dbo.ContactoMensaje', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ContactoMensaje
    (
        Id               INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ContactoMensaje PRIMARY KEY,
        Nombre           NVARCHAR(100)     NOT NULL,
        Correo           NVARCHAR(150)     NOT NULL,
        Mensaje          NVARCHAR(1000)    NOT NULL,
        FechaEnvio       DATETIME2         NOT NULL CONSTRAINT DF_ContactoMensaje_FechaEnvio DEFAULT (SYSUTCDATETIME()),
        Visto            BIT               NOT NULL CONSTRAINT DF_ContactoMensaje_Visto DEFAULT (0),
        Completado       BIT               NOT NULL CONSTRAINT DF_ContactoMensaje_Completado DEFAULT (0),
        FechaVisto       DATETIME2         NULL,
        FechaCompletado  DATETIME2         NULL
    );

    CREATE INDEX IX_ContactoMensaje_Visto_Completado_Fecha
        ON dbo.ContactoMensaje (Visto, Completado, FechaEnvio DESC);
END
GO


-- Insertar mensaje 
IF OBJECT_ID('dbo.SP_GuardarContacto', 'P') IS NOT NULL DROP PROC dbo.SP_GuardarContacto;
GO
CREATE PROC dbo.SP_GuardarContacto
    @Nombre NVARCHAR(100),
    @Correo NVARCHAR(150),
    @Mensaje NVARCHAR(1000),
    @FechaEnvio DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ContactoMensaje (Nombre, Correo, Mensaje, FechaEnvio)
    VALUES (@Nombre, @Correo, @Mensaje, @FechaEnvio);

    RETURN @@ROWCOUNT; -- Dapper usa esto como filas afectadas
END
GO

-- Listar mensajes publicos
IF OBJECT_ID('dbo.SP_ListarContactos', 'P') IS NOT NULL DROP PROC dbo.SP_ListarContactos;
GO
CREATE PROC dbo.SP_ListarContactos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, Correo, Mensaje, FechaEnvio, Visto, Completado, FechaVisto, FechaCompletado
    FROM dbo.ContactoMensaje
    ORDER BY FechaEnvio DESC, Id DESC;
END
GO



-- Listado admin con filtros 
IF OBJECT_ID('dbo.SP_Contacto_ListarAdmin','P') IS NOT NULL
    DROP PROC dbo.SP_Contacto_ListarAdmin;
GO
CREATE PROC dbo.SP_Contacto_ListarAdmin
    @Buscar   NVARCHAR(200) = NULL,
    @Estado   VARCHAR(20)   = NULL,   
    @Page     INT           = 1,
    @PageSize INT           = 20,
    @Total    INT           OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @Buscar = NULLIF(@Buscar, '');
    SET @Estado = LOWER(NULLIF(@Estado, ''));

    SELECT 
        c.Id, c.Nombre, c.Correo, c.Mensaje, c.FechaEnvio,
        c.Visto, c.Completado, c.FechaVisto, c.FechaCompletado
    INTO #Filtro
    FROM dbo.ContactoMensaje c
    WHERE
        (@Buscar IS NULL 
            OR c.Nombre  LIKE '%' + @Buscar + '%'
            OR c.Correo  LIKE '%' + @Buscar + '%'
            OR c.Mensaje LIKE '%' + @Buscar + '%')
        AND
        (
            @Estado IS NULL
            OR (@Estado = 'pendiente'  AND c.Visto = 0 AND c.Completado = 0)
            OR (@Estado = 'visto'      AND c.Visto = 1)
            OR (@Estado = 'completado' AND c.Completado = 1)
        );

    SELECT @Total = COUNT(*) FROM #Filtro;

    SELECT 
        Id, Nombre, Correo, Mensaje, FechaEnvio,
        Visto, Completado, FechaVisto, FechaCompletado
    FROM #Filtro
    ORDER BY FechaEnvio DESC, Id DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Actualizar estado
IF OBJECT_ID('dbo.SP_Contacto_ActualizarEstado','P') IS NOT NULL DROP PROC dbo.SP_Contacto_ActualizarEstado;
GO
CREATE PROC dbo.SP_Contacto_ActualizarEstado
    @Id         INT,
    @Visto      BIT,
    @Completado BIT,
    @Fecha      DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE c
    SET
        Visto = @Visto,
        FechaVisto = CASE
                        WHEN @Visto = 1 AND c.FechaVisto IS NULL THEN @Fecha
                        WHEN @Visto = 0 THEN NULL
                        ELSE c.FechaVisto
                     END,
        Completado = @Completado,
        FechaCompletado = CASE
                            WHEN @Completado = 1 AND c.FechaCompletado IS NULL THEN @Fecha
                            WHEN @Completado = 0 THEN NULL
                            ELSE c.FechaCompletado
                          END
    FROM dbo.ContactoMensaje c
    WHERE c.Id = @Id;

    RETURN @@ROWCOUNT;
END
GO

--cambios de eliminar producto a inactivo
--Columna Activo en Producto (si no existe)
IF COL_LENGTH('dbo.Producto','Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Producto ADD Activo BIT NOT NULL
        CONSTRAINT DF_Producto_Activo DEFAULT(1);
END
GO

---LISTAR (admin)  incluye Activo 
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Listar
    @Page     INT = 1,
    @PageSize INT = 10,
    @Search   NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @s NVARCHAR(202) =
        CASE WHEN @Search IS NULL OR LTRIM(RTRIM(@Search)) = ''
             THEN NULL
             ELSE '%' + LTRIM(RTRIM(@Search)) + '%'
        END;

    DECLARE @q TABLE (
        ProductoID         INT,
        Nombre             NVARCHAR(200),
        Precio             DECIMAL(18,2),
        CategoriaID        INT,
        Categoria          NVARCHAR(200),
        UrlImagenPrincipal NVARCHAR(500) NULL,
        Activo             BIT
    );

    INSERT INTO @q (ProductoID, Nombre, Precio, CategoriaID, Categoria, UrlImagenPrincipal, Activo)
    SELECT  p.ProductoID,
            p.Nombre,
            p.Precio,
            c.CategoriaID,
            c.NombreCategoria AS Categoria,
            (SELECT TOP(1) UrlImagen
               FROM ImagenProducto i
              WHERE i.ProductoID = p.ProductoID
              ORDER BY i.ImagenID),
            p.Activo
    FROM Producto p
    INNER JOIN CategoriaProducto c ON c.CategoriaID = p.CategoriaID
    WHERE (@s IS NULL
           OR p.Nombre       LIKE @s
           OR p.Descripcion  LIKE @s
           OR c.NombreCategoria LIKE @s);

    SELECT COUNT(*) AS Total FROM @q;

    SELECT  ProductoID, Nombre, Precio, CategoriaID, Categoria, UrlImagenPrincipal, Activo
    FROM @q
    ORDER BY ProductoID DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

---DETALLE (admin) — incluye Activo 
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Obtener
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
        p.ProductoID, p.Nombre, p.Descripcion, p.Precio,
        p.CategoriaID, p.MarcaID, p.ProveedorID, p.Activo
    FROM Producto p
    WHERE p.ProductoID = @ProductoID;

    SELECT UrlImagen
    FROM ImagenProducto
    WHERE ProductoID = @ProductoID
    ORDER BY ImagenID;

    SELECT ptc.PTCID, ptc.TallaID, t.NombreTalla, ptc.ColorID, c.NombreColor, ptc.Stock
    FROM ProductoTallaColor ptc
    INNER JOIN Talla t ON t.TallaID = ptc.TallaID
    INNER JOIN Color c ON c.ColorID = ptc.ColorID
    WHERE ptc.ProductoID = @ProductoID;

    SELECT CategoriaID  AS Id, NombreCategoria AS Nombre FROM CategoriaProducto ORDER BY NombreCategoria;
    SELECT MarcaID      AS Id, NombreMarca     AS Nombre FROM Marca            ORDER BY NombreMarca;
    SELECT ProveedorID  AS Id, NombreProveedor AS Nombre FROM Proveedor        ORDER BY NombreProveedor;
    SELECT TallaID      AS Id, NombreTalla     AS Nombre FROM Talla            ORDER BY NombreTalla;
    SELECT ColorID      AS Id, NombreColor     AS Nombre FROM Color            ORDER BY NombreColor;
END
GO

---CREAR — asegura Activo=1 al insertar 
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Crear
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @Precio DECIMAL(18,2),
    @CategoriaID INT,
    @MarcaID INT,
    @ProveedorID INT,
    @Imagenes dbo.ImagenProductoType READONLY,
    @PTCs dbo.PTCType READONLY,
    @NuevoProductoID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        INSERT INTO Producto (Nombre, Descripcion, Precio, CategoriaID, MarcaID, ProveedorID, Activo)
        VALUES (@Nombre, @Descripcion, @Precio, @CategoriaID, @MarcaID, @ProveedorID, 1);

        SET @NuevoProductoID = SCOPE_IDENTITY();

        INSERT INTO ImagenProducto(ProductoID, UrlImagen)
        SELECT @NuevoProductoID, UrlImagen FROM @Imagenes;

        INSERT INTO ProductoTallaColor(ProductoID, TallaID, ColorID, Stock)
        SELECT @NuevoProductoID, TallaID, ColorID, Stock FROM @PTCs;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

--ACTUALIZAR — sin tocar Activo 
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Actualizar
    @ProductoID INT,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @Precio DECIMAL(18,2),
    @CategoriaID INT,
    @MarcaID INT,
    @ProveedorID INT,
    @Imagenes dbo.ImagenProductoType READONLY,
    @PTCs dbo.PTCType READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Producto
        SET Nombre=@Nombre, Descripcion=@Descripcion, Precio=@Precio,
            CategoriaID=@CategoriaID, MarcaID=@MarcaID, ProveedorID=@ProveedorID
        WHERE ProductoID=@ProductoID;

        DELETE FROM ImagenProducto WHERE ProductoID=@ProductoID;
        INSERT INTO ImagenProducto(ProductoID, UrlImagen)
        SELECT @ProductoID, UrlImagen FROM @Imagenes;

        DELETE FROM ProductoTallaColor WHERE ProductoID=@ProductoID;
        INSERT INTO ProductoTallaColor(ProductoID, TallaID, ColorID, Stock)
        SELECT @ProductoID, TallaID, ColorID, Stock FROM @PTCs;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

----INACTIVAR en vez de borrar (soft delete) 
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Eliminar
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Producto SET Activo = 0 WHERE ProductoID = @ProductoID;
END
GO

--ACTIVAR 
CREATE OR ALTER PROCEDURE dbo.usp_Admin_Producto_Activar
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Producto SET Activo = 1 WHERE ProductoID = @ProductoID;
END
GO

----CATÁLOGO público — solo Activos
CREATE OR ALTER PROCEDURE dbo.usp_Producto_Catalogo
    @Busqueda   NVARCHAR(100) = NULL,
    @Categoria  NVARCHAR(100) = NULL,
    @PrecioMin  DECIMAL(18,2) = NULL,
    @PrecioMax  DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.ProductoID,
        p.Nombre,
        LEFT(p.Descripcion, 100) AS DescripcionCorta,
        p.Precio,
        c.NombreCategoria AS Categoria,
        (SELECT TOP (1) UrlImagen 
         FROM ImagenProducto 
         WHERE ProductoID = p.ProductoID 
         ORDER BY ImagenID) AS UrlImagenPrincipal
    FROM Producto p
    INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
    WHERE p.Activo = 1
      AND (@Busqueda IS NULL OR p.Nombre LIKE '%' + @Busqueda + '%')
      AND (@Categoria IS NULL OR c.NombreCategoria = @Categoria)
      AND (@PrecioMin IS NULL OR p.Precio >= @PrecioMin)
      AND (@PrecioMax IS NULL OR p.Precio <= @PrecioMax)
    ORDER BY p.Nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Catalogo_Paginado
 @Busqueda NVARCHAR(100)=NULL, @Categoria NVARCHAR(100)=NULL,
 @PrecioMin DECIMAL(18,2)=NULL, @PrecioMax DECIMAL(18,2)=NULL,
 @Page INT = 1, @PageSize INT = 20
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH Q AS (
    SELECT 
      p.ProductoID, p.Nombre,
      LEFT(p.Descripcion, 100) AS DescripcionCorta,
      p.Precio, c.NombreCategoria AS Categoria,
      (SELECT TOP (1) UrlImagen FROM ImagenProducto WHERE ProductoID = p.ProductoID ORDER BY ImagenID) AS UrlImagenPrincipal
    FROM Producto p
    INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
    WHERE p.Activo = 1
      AND (@Busqueda IS NULL OR p.Nombre LIKE '%' + @Busqueda + '%')
      AND (@Categoria IS NULL OR c.NombreCategoria = @Categoria)
      AND (@PrecioMin IS NULL OR p.Precio >= @PrecioMin)
      AND (@PrecioMax IS NULL OR p.Precio <= @PrecioMax)
  )
  SELECT * FROM Q
  ORDER BY Nombre
  OFFSET (@Page-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;

  SELECT COUNT(*) AS Total FROM Q;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Detalle_Paquete
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Detalle (solo si está activo)
    SELECT p.ProductoID, p.Nombre, p.Descripcion, p.Precio, c.NombreCategoria AS Categoria
    FROM Producto p
    INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
    WHERE p.ProductoID = @ProductoID
      AND p.Activo = 1;

    -- Imágenes
    SELECT UrlImagen 
    FROM ImagenProducto 
    WHERE ProductoID = @ProductoID
      AND EXISTS (SELECT 1 FROM Producto WHERE ProductoID=@ProductoID AND Activo=1)
    ORDER BY ImagenID;

    -- Tallas
    SELECT DISTINCT t.NombreTalla
    FROM ProductoTallaColor ptc
    INNER JOIN Talla t ON ptc.TallaID = t.TallaID
    WHERE ptc.ProductoID = @ProductoID
      AND EXISTS (SELECT 1 FROM Producto WHERE ProductoID=@ProductoID AND Activo=1);

    -- Colores
    SELECT DISTINCT c.NombreColor
    FROM ProductoTallaColor ptc
    INNER JOIN Color c ON ptc.ColorID = c.ColorID
    WHERE ptc.ProductoID = @ProductoID
      AND EXISTS (SELECT 1 FROM Producto WHERE ProductoID=@ProductoID AND Activo=1);

    -- PTC
    SELECT ptc.PTCID, ptc.ProductoID, ptc.TallaID, ptc.ColorID, ptc.Stock, t.NombreTalla, c.NombreColor
    FROM ProductoTallaColor ptc
    INNER JOIN Talla t ON ptc.TallaID = t.TallaID
    INNER JOIN Color c ON ptc.ColorID = c.ColorID
    WHERE ptc.ProductoID = @ProductoID
      AND EXISTS (SELECT 1 FROM Producto WHERE ProductoID=@ProductoID AND Activo=1);
END
GO





-- ===== CATEGORÍAS ===== 
CREATE OR ALTER PROC dbo.usp_Admin_Categoria_Listar
AS
BEGIN
  SELECT CategoriaID AS Id, NombreCategoria AS Nombre
  FROM CategoriaProducto ORDER BY NombreCategoria;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Categoria_Crear
  @Nombre NVARCHAR(100), @NuevoId INT OUTPUT
AS
BEGIN
  INSERT INTO CategoriaProducto(NombreCategoria) VALUES(@Nombre);
  SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Categoria_Actualizar
  @Id INT, @Nombre NVARCHAR(100)
AS
BEGIN
  UPDATE CategoriaProducto SET NombreCategoria=@Nombre WHERE CategoriaID=@Id;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Categoria_Eliminar
  @Id INT
AS
BEGIN
  IF EXISTS(SELECT 1 FROM Producto WHERE CategoriaID=@Id)
     RAISERROR('CATEGORIA_EN_USO',16,1);
  ELSE
     DELETE FROM CategoriaProducto WHERE CategoriaID=@Id;
END
GO

-- ===== MARCAS ===== 
CREATE OR ALTER PROC dbo.usp_Admin_Marca_Listar
AS
BEGIN
  SELECT MarcaID AS Id, NombreMarca AS Nombre
  FROM Marca ORDER BY NombreMarca;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Marca_Crear
  @Nombre NVARCHAR(100), @NuevoId INT OUTPUT
AS
BEGIN
  INSERT INTO Marca(NombreMarca) VALUES(@Nombre);
  SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Marca_Actualizar
  @Id INT, @Nombre NVARCHAR(100)
AS
BEGIN
  UPDATE Marca SET NombreMarca=@Nombre WHERE MarcaID=@Id;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Marca_Eliminar
  @Id INT
AS
BEGIN
  IF EXISTS(SELECT 1 FROM Producto WHERE MarcaID=@Id)
     RAISERROR('MARCA_EN_USO',16,1);
  ELSE
     DELETE FROM Marca WHERE MarcaID=@Id;
END
GO

---===== TALLAS ===== 
CREATE OR ALTER PROC dbo.usp_Admin_Talla_Listar
AS
BEGIN
  SELECT TallaID AS Id, NombreTalla AS Nombre
  FROM Talla ORDER BY NombreTalla;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Talla_Crear
  @Nombre NVARCHAR(10), @NuevoId INT OUTPUT
AS
BEGIN
  INSERT INTO Talla(NombreTalla) VALUES(@Nombre);
  SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Talla_Actualizar
  @Id INT, @Nombre NVARCHAR(10)
AS
BEGIN
  UPDATE Talla SET NombreTalla=@Nombre WHERE TallaID=@Id;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Talla_Eliminar
  @Id INT
AS
BEGIN
  IF EXISTS(SELECT 1 FROM ProductoTallaColor WHERE TallaID=@Id)
     RAISERROR('TALLA_EN_USO',16,1);
  ELSE
     DELETE FROM Talla WHERE TallaID=@Id;
END
GO

----===== COLORES ===== 
CREATE OR ALTER PROC dbo.usp_Admin_Color_Listar
AS
BEGIN
  SELECT ColorID AS Id, NombreColor AS Nombre
  FROM Color ORDER BY NombreColor;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Color_Crear
  @Nombre NVARCHAR(50), @NuevoId INT OUTPUT
AS
BEGIN
  INSERT INTO Color(NombreColor) VALUES(@Nombre);
  SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Color_Actualizar
  @Id INT, @Nombre NVARCHAR(50)
AS
BEGIN
  UPDATE Color SET NombreColor=@Nombre WHERE ColorID=@Id;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Color_Eliminar
  @Id INT
AS
BEGIN
  IF EXISTS(SELECT 1 FROM ProductoTallaColor WHERE ColorID=@Id)
     RAISERROR('COLOR_EN_USO',16,1);
  ELSE
     DELETE FROM Color WHERE ColorID=@Id;
END
GO

--===== PROVEEDORES ===== 
CREATE OR ALTER PROC dbo.usp_Admin_Proveedor_Listar
AS
BEGIN
  SELECT ProveedorID AS Id, NombreProveedor, Correo, Telefono
  FROM Proveedor ORDER BY NombreProveedor;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Proveedor_Crear
  @Nombre NVARCHAR(100), @Correo NVARCHAR(100), @Telefono NVARCHAR(50),
  @NuevoId INT OUTPUT
AS
BEGIN
  INSERT INTO Proveedor(NombreProveedor,Correo,Telefono)
  VALUES(@Nombre,@Correo,@Telefono);
  SET @NuevoId = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Proveedor_Actualizar
  @Id INT, @Nombre NVARCHAR(100), @Correo NVARCHAR(100), @Telefono NVARCHAR(50)
AS
BEGIN
  UPDATE Proveedor
  SET NombreProveedor=@Nombre, Correo=@Correo, Telefono=@Telefono
  WHERE ProveedorID=@Id;
END
GO

CREATE OR ALTER PROC dbo.usp_Admin_Proveedor_Eliminar
  @Id INT
AS
BEGIN
  IF EXISTS(SELECT 1 FROM Producto WHERE ProveedorID=@Id)
     RAISERROR('PROVEEDOR_EN_USO',16,1);
  ELSE
     DELETE FROM Proveedor WHERE ProveedorID=@Id;
END
GO

---
UPDATE Producto
SET Nombre = 'Pantalón Mujer', Descripcion = 'Pantalón de mujer negro casual'
WHERE Nombre = 'Pantalon Mujer';

UPDATE Producto
SET Nombre = 'Pantalón Zara Hombre'
WHERE Nombre = 'Pantalon Zara Hombre';