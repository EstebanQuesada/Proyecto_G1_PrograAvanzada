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

