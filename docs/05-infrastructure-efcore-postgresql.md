# Infrastructure - Entity Framework Core y PostgreSQL

## Objetivo

En este entregable se creó la capa Infrastructure para conectar el sistema con PostgreSQL usando Entity Framework Core.

El objetivo fue implementar persistencia real para productos.

## Responsabilidad de Infrastructure

Infrastructure contiene implementaciones concretas de detalles técnicos.

En este entregable incluye:

- Acceso a base de datos.
- Configuración de Entity Framework Core.
- AppDbContext.
- Configuración de la entidad Product.
- Implementación concreta de IProductRepository.
- Conexión con PostgreSQL.

## Archivos principales

Los archivos principales creados fueron:

```text
backend/src/Ecommerce.Infrastructure/Persistence/AppDbContext.cs
backend/src/Ecommerce.Infrastructure/Persistence/Configurations/ProductConfiguration.cs
backend/src/Ecommerce.Infrastructure/Repositories/ProductRepository.cs
backend/src/Ecommerce.Infrastructure/DependencyInjection/DependencyInjection.cs
backend/src/Ecommerce.Application/DependencyInjection/DependencyInjection.cs
```

## Docker y PostgreSQL local

Se creó un archivo `docker-compose.yml` en la raíz del proyecto para levantar PostgreSQL local.

Servicio principal:

```text
postgres
```

Datos de desarrollo:

```text
Database: ecommerce_db
User: ecommerce_user
Port: 5432
```

La contraseña utilizada es solo para entorno local de desarrollo.

## Comandos Docker utilizados

Para levantar PostgreSQL:

```powershell
docker compose up -d
```

Para verificar que el contenedor esté corriendo:

```powershell
docker ps
```

Para apagar los servicios sin borrar datos:

```powershell
docker compose down
```

Para apagar y borrar también el volumen de datos:

```powershell
docker compose down -v
```

## AppDbContext

`AppDbContext` es el puente entre Entity Framework Core y la base de datos.

Incluye:

```csharp
public DbSet<Product> Products => Set<Product>();
```

Esto representa la tabla de productos dentro de la base de datos.

## ProductConfiguration

`ProductConfiguration` define cómo se mapea la entidad `Product` en la base de datos.

Configura aspectos como:

- Nombre de tabla.
- Primary key.
- Longitud máxima de campos.
- Tipo decimal para precio.
- Campos obligatorios.
- Índice por nombre.

## ProductRepository

`ProductRepository` implementa la interfaz `IProductRepository`.

Esta clase usa `AppDbContext` para realizar operaciones reales sobre PostgreSQL.

Implementa operaciones como:

- GetAllAsync.
- GetByIdAsync.
- AddAsync.
- UpdateAsync.
- DeleteAsync.

## Registro de dependencias

Se creó un método de extensión en Infrastructure:

```csharp
builder.Services.AddInfrastructure(builder.Configuration);
```

Este método registra:

- AppDbContext.
- ProductRepository.
- Configuración de PostgreSQL.

También se creó un método de extensión en Application:

```csharp
builder.Services.AddApplication();
```

Este método registra:

- IProductService.
- ProductService.

## Connection string

La cadena de conexión se configuró en:

```text
backend/src/Ecommerce.Api/appsettings.Development.json
```

Ejemplo local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ecommerce_db;Username=ecommerce_user;Password=ecommerce_password"
  }
}
```

## Migraciones

Se creó la migración inicial con:

```powershell
dotnet ef migrations add InitialCreate --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api --output-dir Persistence\Migrations
```

Se aplicó a la base de datos con:

```powershell
dotnet ef database update --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api
```

## Tablas creadas

La migración inicial crea, entre otras, las siguientes tablas:

```text
Products
__EFMigrationsHistory
```

## Por qué AppDbContext está en Infrastructure

`AppDbContext` depende de Entity Framework Core, que es una tecnología concreta.

Por eso no debe estar en Domain ni en Application.

## Por qué ProductRepository está en Infrastructure

Application define el contrato `IProductRepository`, pero no sabe cómo se guardan los productos.

Infrastructure implementa ese contrato usando PostgreSQL y Entity Framework Core.

Esto respeta la dirección de dependencias de Clean Architecture.

## Resultado

Al finalizar este entregable, el sistema quedó conectado a PostgreSQL y preparado para guardar productos reales en una base de datos local con Docker.