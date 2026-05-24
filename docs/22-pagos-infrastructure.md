# Pagos - Infrastructure

## Objetivo

En este entregable se implementó la persistencia real de pagos usando Entity Framework Core y PostgreSQL.

El objetivo fue conectar el contrato `IPaymentRepository`, definido en Application, con una implementación concreta dentro de Infrastructure.

## Archivos creados o modificados

Se modificaron:

```text
backend/src/Ecommerce.Infrastructure/Persistence/AppDbContext.cs
backend/src/Ecommerce.Infrastructure/DependencyInjection/DependencyInjection.cs
```

Se crearon:

```text
backend/src/Ecommerce.Infrastructure/Persistence/Configurations/PaymentConfiguration.cs
backend/src/Ecommerce.Infrastructure/Repositories/PaymentRepository.cs
```

## AppDbContext

Se agregó el DbSet:

```csharp
public DbSet<Payment> Payments => Set<Payment>();
```

Esto permite que Entity Framework Core trabaje con la entidad `Payment`.

## PaymentConfiguration

`PaymentConfiguration` define cómo se mapea la entidad `Payment` en la base de datos.

Configura:

- Tabla `Payments`.
- Primary key.
- OrderId.
- Amount.
- Method.
- Status.
- ExternalReference.
- CreatedAt.
- ApprovedAt.
- RejectedAt.
- CancelledAt.
- Índices por OrderId, Status y Method.

## Enums como enteros

Los enums `PaymentMethod` y `PaymentStatus` se guardan como enteros mediante:

```csharp
.HasConversion<int>();
```

Esto permite persistir valores controlados sin guardar textos arbitrarios.

## Relación con Order

`Payment` guarda el identificador de la orden mediante:

```csharp
OrderId
```

En esta etapa no se configuró una navegación directa entre `Payment` y `Order`.

Esto mantiene el dominio simple y evita acoplar la entidad de pago a detalles de persistencia.

## PaymentRepository

Se creó:

```text
backend/src/Ecommerce.Infrastructure/Repositories/PaymentRepository.cs
```

Este repositorio implementa:

```text
IPaymentRepository
```

y usa:

```text
AppDbContext
```

para guardar, buscar, listar y actualizar pagos.

## Métodos implementados

`PaymentRepository` implementa:

```csharp
Task<IReadOnlyList<Payment>> GetAllAsync();
Task<Payment?> GetByIdAsync(Guid id);
Task<IReadOnlyList<Payment>> GetByOrderIdAsync(Guid orderId);
Task AddAsync(Payment payment);
Task UpdateAsync(Payment payment);
```

## Registro de dependencias

Se registró la implementación:

```csharp
services.AddScoped<IPaymentRepository, PaymentRepository>();
```

Con esto, cuando Application solicite `IPaymentRepository`, se usará `PaymentRepository`.

## Migración

Se creó una migración con:

```powershell
dotnet ef migrations add AddPaymentTable --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api --output-dir Persistence\Migrations
```

Y se aplicó con:

```powershell
dotnet ef database update --project src\Ecommerce.Infrastructure --startup-project src\Ecommerce.Api
```

## Tabla creada

La migración crea:

```text
Payments
```

## Resultado

Al finalizar este entregable, los pagos ya pueden persistirse en PostgreSQL mediante Entity Framework Core.

Todavía no existen endpoints HTTP para operar con pagos desde Postman o Swagger. Eso se implementará en el siguiente entregable.
