# Checkout - Application de órdenes

## Objetivo

En este entregable se creó la capa Application para órdenes de compra.

El objetivo fue definir los casos de uso necesarios para convertir un carrito en una orden y administrar el estado inicial de esa orden.

## Archivos creados

Se crearon DTOs, interfaces y servicios relacionados con órdenes.

## DTOs creados

```text
backend/src/Ecommerce.Application/DTOs/OrderDto.cs
backend/src/Ecommerce.Application/DTOs/OrderItemDto.cs
backend/src/Ecommerce.Application/DTOs/CreateOrderDto.cs
```

## Interfaces creadas

```text
backend/src/Ecommerce.Application/Interfaces/IOrderRepository.cs
backend/src/Ecommerce.Application/Interfaces/IOrderService.cs
```

## Servicio creado

```text
backend/src/Ecommerce.Application/Services/OrderService.cs
```

## Responsabilidad de OrderService

`OrderService` coordina los casos de uso relacionados con órdenes.

Permite:

- Crear una orden desde un carrito.
- Obtener una orden por Id.
- Listar órdenes.
- Confirmar una orden.
- Cancelar una orden.

## Flujo de creación de una orden

El flujo principal es:

```text
CreateOrderDto
        ↓
OrderService
        ↓
ICartRepository
        ↓
Cart
        ↓
OrderItem
        ↓
Order
        ↓
IOrderRepository
```

## Crear orden desde carrito

Para crear una orden, el servicio busca el carrito por Id.

Si el carrito no existe, lanza una `DomainException`.

Si el carrito está vacío, también lanza una `DomainException`.

Si el carrito es válido, se copian sus items a nuevos `OrderItem`.

## Por qué se copian los datos del carrito

La orden debe conservar una foto histórica de la compra.

Por eso se copian:

- ProductId.
- ProductName.
- UnitPrice.
- Quantity.

Aunque el producto cambie luego en el catálogo, la orden conserva los datos del momento de compra.

## Estados de orden

Desde Application se pueden ejecutar operaciones para:

- Confirmar orden.
- Cancelar orden.

Las reglas de transición siguen estando en el dominio, dentro de `Order`.

## Tests

Se crearon tests en:

```text
backend/tests/Ecommerce.Tests/Application/OrderServiceTests.cs
```

Los tests verifican:

- Crear una orden desde un carrito con items.
- Evitar crear una orden desde un carrito inexistente.
- Evitar crear una orden desde un carrito vacío.
- Obtener una orden por Id.
- Listar órdenes.
- Confirmar una orden.
- Cancelar una orden.

## Resultado

Al finalizar este entregable, el sistema cuenta con casos de uso de órdenes listos para conectarse con Infrastructure y API.
