# Checkout - Infrastructure de órdenes

## Objetivo

En este entregable se implementó la persistencia real de órdenes de compra usando Entity Framework Core y PostgreSQL.

El objetivo fue conectar el contrato `IOrderRepository`, definido en Application, con una implementación concreta dentro de Infrastructure.

## Archivos creados o modificados

Se modificó:

```text
backend/src/Ecommerce.Infrastructure/Persistence/AppDbContext.cs
backend/src/Ecommerce.Infrastructure/DependencyInjection/DependencyInjection.cs
```

Se crearon:

```text
backend/src/Ecommerce.Infrastructure/Persistence/Configurations/OrderConfiguration.cs
backend/src/Ecommerce.Infrastructure/Persistence/Configurations/OrderItemConfiguration.cs
backend/src/Ecommerce.Infrastructure/Repositories/OrderRepository.cs
```

## AppDbContext

Se agregó el DbSet:

```csharp
public DbSet<Order> Orders => Set<Order>();
```

Esto permite que Entity Framework Core trabaje con la entidad `Order`.

No se agregó un `DbSet<OrderItem>` público porque los items se manejan a través del agregado `Order`.

## OrderConfiguration

`OrderConfiguration` define cómo se mapea la entidad `Order` en la base de datos.

Configura:

- Tabla `Orders`.
- Primary key.
- CartId.
- CustomerName.
- CustomerEmail.
- Status.
- CreatedAt.
- ConfirmedAt.
- CancelledAt.
- Relación uno a muchos con `OrderItem`.
- Eliminación en cascada de items.
- Acceso mediante backing field para la colección de items.
- Índices por CartId, CustomerEmail y Status.

## OrderItemConfiguration

`OrderItemConfiguration` define cómo se mapea cada item de la orden.

Configura:

- Tabla `OrderItems`.
- Primary key.
- ProductId.
- ProductName.
- UnitPrice.
- Quantity.
- Índices.
- Shadow property `OrderId`.

## Shadow property OrderId

`OrderId` se configuró como una shadow property.

Esto significa que existe en la base de datos, pero no está expuesta como propiedad pública en la entidad `OrderItem`.

La relación se mantiene a nivel de infraestructura sin contaminar el dominio con detalles de persistencia.

## Persistencia histórica

`OrderItem` guarda una copia del nombre y precio del producto.

Esto es importante porque una orden debe conservar los datos históricos de la compra, aunque el producto cambie luego en el catálogo.

## OrderRepository

Se creó:

```text
backend/src/Ecommerce.Infrastructure/Repositories/OrderRepository.cs
```

Este repositorio implementa:

```text
IOrderRepository
```

y usa:

```text
AppDbContext
```

para guardar, buscar, listar y actualizar órdenes.

## Métodos implementados

`OrderRepository` implementa:

```csharp
Task<IReadOnlyList<Order>> GetAllAsync();
Task<Order?> GetByIdAsync(Guid id);
Task AddAsync(Order order);
Task UpdateAsync(Order order);
```

## Include de Items

Al buscar una orden por Id o listar órdenes, se cargan también sus items:

```csharp
_context.Orders
    .Include(order => order.Items)
```

Esto permite reconstruir la orden completa desde PostgreSQL.

## Registro de dependencias

Se registró la implementación:

```csharp
services.AddScoped<IOrderRepository, OrderRepository>();
```

Con esto, cuando Application solicite `IOrderRepository`, se usará `OrderRepository`.

## Migración

Se creó una migración con:

```powershell
dotnet ef migrations add AddOrderTables --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api --output-dir Persistence\Migrations
```

Y se aplicó con:

```powershell
dotnet ef database update --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api
```

## Tablas creadas

La migración crea:

```text
Orders
OrderItems
```

## Relación entre tablas

La relación es:

```text
Orders 1 ─── * OrderItems
```

Una orden puede tener muchos items.

Cada item pertenece a una orden.

## Resultado

Al finalizar este entregable, las órdenes ya pueden persistirse en PostgreSQL mediante Entity Framework Core.

Todavía no existen endpoints HTTP para operar con órdenes desde Swagger. Eso se implementará en el siguiente entregable.
