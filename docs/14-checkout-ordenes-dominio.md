# Checkout - Dominio de órdenes

## Objetivo

En este entregable se creó el dominio inicial de órdenes de compra.

El objetivo fue modelar las entidades necesarias para representar una compra generada a partir de un carrito.

## Entidades creadas

Se crearon:

```text
backend/src/Ecommerce.Domain/Entities/Order.cs
backend/src/Ecommerce.Domain/Entities/OrderItem.cs
```

También se creó:

```text
backend/src/Ecommerce.Domain/Enums/OrderStatus.cs
```

## Order

`Order` representa una orden de compra.

Contiene:

- Id.
- CartId.
- CustomerName.
- CustomerEmail.
- Items.
- Status.
- CreatedAt.
- ConfirmedAt.
- CancelledAt.

También expone propiedades calculadas:

- Total.
- TotalItems.

## OrderItem

`OrderItem` representa un producto dentro de una orden.

Contiene:

- Id.
- ProductId.
- ProductName.
- UnitPrice.
- Quantity.
- Subtotal.

## OrderStatus

`OrderStatus` representa el estado de una orden.

Estados iniciales:

```text
Pending
Confirmed
Cancelled
```

## Reglas de negocio

El dominio de órdenes protege las siguientes reglas:

- El carrito asociado es obligatorio.
- El nombre del cliente es obligatorio.
- El email del cliente es obligatorio.
- El email debe tener un formato básico válido.
- La orden debe tener al menos un producto.
- El producto de un item es obligatorio.
- El nombre del producto es obligatorio.
- El precio unitario debe ser mayor que cero.
- La cantidad debe ser mayor que cero.
- Una orden cancelada no puede confirmarse.
- Una orden confirmada no puede cancelarse.
- Una orden no puede confirmarse dos veces.
- Una orden no puede cancelarse dos veces.

## Por qué OrderItem guarda nombre y precio

Una orden debe conservar una foto histórica de la compra.

Si el precio o el nombre del producto cambian en el catálogo después de comprar, la orden debe seguir mostrando los datos del momento de la compra.

Ejemplo:

```text
El cliente compra Mouse Logitech a $50.
Luego el producto sube a $70.
La orden debe seguir mostrando $50.
```

## Por qué usamos OrderStatus

Usamos un enum para evitar estados arbitrarios en la orden.

Esto permite que el dominio solo trabaje con estados válidos.

## Flujo conceptual

```text
Carrito con items
        ↓
Checkout
        ↓
Order
        ↓
OrderItem
        ↓
OrderStatus.Pending
```

## Tests

Se crearon tests en:

```text
backend/tests/Ecommerce.Tests/Domain/OrderTests.cs
backend/tests/Ecommerce.Tests/Domain/OrderItemTests.cs
```

Los tests validan:

- Crear una orden válida.
- Crear items válidos.
- Evitar datos inválidos.
- Calcular total.
- Calcular total de items.
- Confirmar orden.
- Cancelar orden.
- Evitar transiciones inválidas.

## Resultado

Al finalizar este entregable, el sistema cuenta con el dominio inicial necesario para representar órdenes de compra.

Todavía no se conecta con Application, Infrastructure ni API. Eso se implementará en entregables posteriores.
