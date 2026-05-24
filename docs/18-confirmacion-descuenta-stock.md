# Confirmación de orden y descuento de stock

## Objetivo

En este entregable se modificó el flujo de confirmación de órdenes para que afecte el stock real del catálogo.

El objetivo fue lograr que una orden confirmada descuente del inventario los productos comprados.

## Problema anterior

Antes de este entregable, una orden podía confirmarse, pero el stock de los productos no cambiaba.

Esto no representaba correctamente el comportamiento esperado de un e-commerce.

Ejemplo:

```text
Stock inicial: 15
Orden confirmada por 2 unidades
Stock final anterior: 15
```

El stock debía quedar en:

```text
Stock final esperado: 13
```

## Archivos modificados

Se modificaron:

```text
backend/src/Ecommerce.Application/Services/OrderService.cs
backend/tests/Ecommerce.Tests/Application/OrderServiceTests.cs
```

## Nueva dependencia de OrderService

`OrderService` ahora depende de:

```text
IOrderRepository
ICartRepository
IProductRepository
```

Esto permite que, al confirmar una orden, el servicio pueda buscar los productos reales del catálogo.

## Flujo de confirmación actualizado

El flujo actualizado es:

```text
PATCH /api/orders/{id}/confirm
        ↓
OrdersController
        ↓
OrderService.ConfirmAsync
        ↓
IOrderRepository.GetByIdAsync
        ↓
Validar productos y stock
        ↓
Descontar stock con Product.DecreaseStock
        ↓
IProductRepository.UpdateAsync
        ↓
Order.Confirm
        ↓
IOrderRepository.UpdateAsync
```

## Validaciones agregadas

Antes de descontar stock, el servicio valida:

- Que la orden exista.
- Que cada producto de la orden siga existiendo.
- Que cada producto siga activo.
- Que haya stock suficiente.
- Que la orden no esté confirmada previamente.
- Que la orden no esté cancelada.

## Evitar descuentos parciales

El servicio primero valida todos los productos.

Recién después descuenta stock.

Esto evita escenarios donde una orden con varios productos descuente stock de algunos productos y luego falle con otro.

## Evitar descontar stock dos veces

Si una orden ya está confirmada, `OrderService` corta el flujo antes de descontar stock y lanza una `DomainException`.

Además, el dominio también protege la regla para evitar confirmar dos veces una orden.

Esto evita confirmar dos veces la misma orden y descontar stock dos veces.

## Tests

Se actualizaron los tests de `OrderService`.

Se agregaron pruebas para validar:

- Confirmar una orden descuenta stock.
- Confirmar una orden inexistente devuelve `null`.
- Confirmar con producto inexistente lanza error.
- Confirmar con producto inactivo lanza error.
- Confirmar con stock insuficiente lanza error.
- Confirmar dos veces no descuenta stock dos veces.

## Resultado

Al finalizar este entregable, confirmar una orden modifica el inventario real del catálogo.

El sistema se acerca más al comportamiento esperado de un e-commerce productivo.
