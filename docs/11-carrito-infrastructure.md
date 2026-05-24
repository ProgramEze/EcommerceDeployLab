# Carrito de compras - Infrastructure

## Objetivo

En este entregable se implementó la persistencia real del carrito de compras usando Entity Framework Core y PostgreSQL.

El objetivo fue conectar el contrato `ICartRepository`, definido en Application, con una implementación concreta dentro de Infrastructure.

## Archivos creados o modificados

Se modificó:

```text
backend/src/Ecommerce.Infrastructure/Persistence/AppDbContext.cs
```

Se crearon:

```text
backend/src/Ecommerce.Infrastructure/Persistence/Configurations/CartConfiguration.cs
backend/src/Ecommerce.Infrastructure/Persistence/Configurations/CartItemConfiguration.cs
backend/src/Ecommerce.Infrastructure/Repositories/CartRepository.cs
```

También se modificó:

```text
backend/src/Ecommerce.Infrastructure/DependencyInjection/DependencyInjection.cs
```

## AppDbContext

Se agregó el DbSet:

```csharp
public DbSet<Cart> Carts => Set<Cart>();
```

Esto permite que Entity Framework Core trabaje con la entidad `Cart`.

No se agregó un `DbSet<CartItem>` público porque los items se manejan a través del agregado `Cart`.

## CartConfiguration

`CartConfiguration` define cómo se mapea la entidad `Cart` en la base de datos.

Configura:

- Tabla `Carts`.
- Primary key.
- CreatedAt.
- UpdatedAt.
- Relación uno a muchos con `CartItem`.
- Eliminación en cascada de items.
- Acceso mediante backing field para la colección de items.

## Protección de Items

En el dominio, `Cart` protege su colección interna usando:

```csharp
private readonly List<CartItem> _items = new();

public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
```

Esto impide que otras capas modifiquen directamente los items.

Para que Entity Framework Core pueda cargar esa colección, se configuró:

```csharp
builder.Navigation(cart => cart.Items)
    .UsePropertyAccessMode(PropertyAccessMode.Field);
```

## CartItemConfiguration

`CartItemConfiguration` define cómo se mapea cada item del carrito.

Configura:

- Tabla `CartItems`.
- Primary key.
- ProductId.
- ProductName.
- UnitPrice.
- Quantity.
- Índices.
- Shadow property `CartId`.

## Shadow property CartId

`CartId` se configuró como una shadow property.

Esto significa que existe en la base de datos, pero no está expuesta como propiedad pública en la entidad `CartItem`.

La relación se mantiene a nivel de infraestructura sin contaminar el dominio con detalles de persistencia.

## CartRepository

Se creó:

```text
backend/src/Ecommerce.Infrastructure/Repositories/CartRepository.cs
```

Este repositorio implementa:

```text
ICartRepository
```

y usa:

```text
AppDbContext
```

para guardar, buscar, actualizar y eliminar carritos.

## Métodos implementados

`CartRepository` implementa:

```csharp
Task<Cart?> GetByIdAsync(Guid id);
Task AddAsync(Cart cart);
Task UpdateAsync(Cart cart);
Task DeleteAsync(Cart cart);
```

## Include de Items

Al buscar un carrito por Id, se cargan también sus items:

```csharp
_context.Carts
    .Include(cart => cart.Items)
    .FirstOrDefaultAsync(cart => cart.Id == id);
```

Esto permite reconstruir el carrito completo desde PostgreSQL.

## Registro de dependencias

Se registró la implementación:

```csharp
services.AddScoped<ICartRepository, CartRepository>();
```

Con esto, cuando Application solicite `ICartRepository`, se usará `CartRepository`.

## Migración

Se creó una migración con:

```powershell
dotnet ef migrations add AddCartTables --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api --output-dir Persistence\Migrations
```

Y se aplicó con:

```powershell
dotnet ef database update --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api
```

## Tablas creadas

La migración crea:

```text
Carts
CartItems
```

## Relación entre tablas

La relación es:

```text
Carts 1 ─── * CartItems
```

Un carrito puede tener muchos items.

Cada item pertenece a un carrito.

## Resultado

Al finalizar este entregable, el carrito ya puede persistirse en PostgreSQL mediante Entity Framework Core.

Todavía no existen endpoints HTTP para usarlo desde Swagger. Eso se implementará en el siguiente entregable.
