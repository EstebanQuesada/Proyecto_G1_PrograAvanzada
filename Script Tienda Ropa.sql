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

INSERT INTO Rol (NombreRol) VALUES ('Cliente');
INSERT INTO Rol (NombreRol) VALUES ('Administrador');
