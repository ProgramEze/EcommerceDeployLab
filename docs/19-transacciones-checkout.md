# Transacciones en checkout

## Objetivo

En este entregable se incorporó el uso de transacciones en el flujo de confirmación de órdenes.

El objetivo fue asegurar que la confirmación de una orden y el descuento de stock se ejecuten como una única operación atómica.

## Problema anterior

Antes de este entregable, el flujo de confirmación realizaba varias operaciones relacionadas:

```text
Buscar orden
Validar estado
Validar productos
Descontar stock
Confirmar orden
Guardar cambios
```

Si ocurría un error en el medio del proceso, podía existir riesgo de dejar el sistema en un estado inconsistente.

Por ejemplo:

```text
Stock descontado, pero orden no confirmada.
```

## Solución implementada

Se creó una abstracción llamada:

```text
IUnitOfWork
```

Esta interfaz permite ejecutar una operación dentro de una transacción sin que Application dependa directamente de Entity Framework Core.

## Archivos creados o modificados

Se creó:

```text
backend/src/Ecommerce.Application/Interfaces/IUnitOfWork.cs
backend/src/Ecommerce.Infrastructure/Persistence/EfUnitOfWork.cs
```

Se modificaron:

```text
backend/src/Ecommerce.Application/Services/OrderService.cs
backend/src/Ecommerce.Infrastructure/DependencyInjection/DependencyInjection.cs
backend/tests/Ecommerce.Tests/Application/OrderServiceTests.cs
```

## IUnitOfWork

`IUnitOfWork` define el contrato:

```csharp
Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
```

Esto permite que Application solicite ejecutar una operación de forma transaccional.

## EfUnitOfWork

`EfUnitOfWork` implementa `IUnitOfWork` usando Entity Framework Core.

Internamente usa:

```csharp
_context.Database.BeginTransactionAsync();
```

Si todo sale bien, realiza:

```csharp
transaction.CommitAsync();
```

Si ocurre una excepción, realiza:

```csharp
transaction.RollbackAsync();
```

## Uso en OrderService

El método `ConfirmAsync` de `OrderService` ahora ejecuta su lógica dentro de una transacción.

Esto incluye:

- Buscar la orden.
- Validar el estado.
- Validar productos.
- Descontar stock.
- Confirmar la orden.
- Guardar cambios.

## Flujo actualizado

```text
PATCH /api/orders/{id}/confirm
        ↓
OrdersController
        ↓
OrderService.ConfirmAsync
        ↓
IUnitOfWork.ExecuteInTransactionAsync
        ↓
IOrderRepository.GetByIdAsync
        ↓
Validar orden
        ↓
Validar productos
        ↓
Product.DecreaseStock
        ↓
IProductRepository.UpdateAsync
        ↓
Order.Confirm
        ↓
IOrderRepository.UpdateAsync
        ↓
Commit / Rollback
```

## Por qué Application no usa DbContext directamente

Application no debe depender de Entity Framework Core ni de PostgreSQL.

Por eso se define `IUnitOfWork` como contrato en Application y se implementa en Infrastructure.

Esto mantiene la separación de responsabilidades de Clean Architecture.

## Tests

Se actualizaron los tests de `OrderService` con un `FakeUnitOfWork`.

El fake no abre una transacción real, porque los tests unitarios no usan base de datos.

Simplemente ejecuta la acción:

```csharp
return await action();
```

## Resultado

Al finalizar este entregable, la confirmación de órdenes se ejecuta dentro de una transacción.

Esto mejora la consistencia del checkout y prepara el sistema para escenarios más reales de compra.
