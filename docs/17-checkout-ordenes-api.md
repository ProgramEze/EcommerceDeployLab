# Checkout - API de órdenes

## Objetivo

En este entregable se expusieron endpoints HTTP para operar con órdenes de compra.

El objetivo fue conectar la capa `Ecommerce.Api` con los casos de uso de órdenes definidos en `Ecommerce.Application` y con la persistencia real implementada en `Ecommerce.Infrastructure`.

Con esto, el sistema permite crear órdenes desde carritos reales, consultar órdenes, listar órdenes, confirmar órdenes y cancelar órdenes.

## Controlador creado

Se creó el controlador:

```text
backend/src/Ecommerce.Api/Controllers/OrdersController.cs
```

Este controlador usa la interfaz:

```text
IOrderService
```

para acceder a los casos de uso de órdenes.

## Endpoints creados

```http
GET    /api/orders
GET    /api/orders/{id}
POST   /api/orders
PATCH  /api/orders/{id}/confirm
PATCH  /api/orders/{id}/cancel
```

## GET /api/orders

Devuelve todas las órdenes registradas.

Ejemplo:

```http
GET /api/orders
```

Respuesta esperada:

```http
200 OK
```

Devuelve una lista de órdenes ordenadas desde las más recientes hacia las más antiguas.

## GET /api/orders/{id}

Devuelve una orden por Id.

Ejemplo:

```http
GET /api/orders/{id}
```

Si la orden existe, devuelve:

```http
200 OK
```

Si la orden no existe, devuelve:

```http
404 Not Found
```

Con una respuesta como:

```json
{
	"message": "Orden no encontrada."
}
```

## POST /api/orders

Crea una orden desde un carrito existente.

Ejemplo:

```http
POST /api/orders
```

Body esperado:

```json
{
	"cartId": "guid-del-carrito",
	"customerName": "Ezequiel Díaz",
	"customerEmail": "ezequiel@example.com"
}
```

Si el carrito existe y tiene items, se crea una orden en estado `Pending`.

Respuesta esperada:

```http
201 Created
```

Ejemplo de respuesta:

```json
{
	"id": "guid-de-la-orden",
	"cartId": "guid-del-carrito",
	"customerName": "Ezequiel Díaz",
	"customerEmail": "ezequiel@example.com",
	"items": [
		{
			"productId": "guid-del-producto",
			"productName": "Mouse Logitech",
			"unitPrice": 50,
			"quantity": 2,
			"subtotal": 100
		}
	],
	"status": 1,
	"total": 100,
	"totalItems": 2,
	"createdAt": "2026-05-24T00:00:00Z",
	"confirmedAt": null,
	"cancelledAt": null
}
```

## Estados de orden

El enum `OrderStatus` se representa inicialmente con valores numéricos:

```text
1 = Pending
2 = Confirmed
3 = Cancelled
```

## PATCH /api/orders/{id}/confirm

Confirma una orden pendiente.

Ejemplo:

```http
PATCH /api/orders/{id}/confirm
```

Si la orden existe y está pendiente, devuelve la orden con estado `Confirmed`.

Si la orden ya está confirmada, el dominio lanza una `DomainException`.

Si la orden está cancelada, el dominio también impide confirmarla.

## PATCH /api/orders/{id}/cancel

Cancela una orden pendiente.

Ejemplo:

```http
PATCH /api/orders/{id}/cancel
```

Si la orden existe y está pendiente, devuelve la orden con estado `Cancelled`.

Si la orden ya está cancelada, el dominio lanza una `DomainException`.

Si la orden está confirmada, el dominio impide cancelarla.

## Relación con Application

`OrdersController` no manipula directamente entidades del dominio ni usa Entity Framework Core.

El controller llama a:

```text
IOrderService
```

La implementación concreta es:

```text
OrderService
```

Esto permite mantener la API separada de la lógica de negocio.

## Relación con Domain

Las reglas principales de órdenes siguen viviendo en el dominio, dentro de:

```text
Order
OrderItem
OrderStatus
```

Por ejemplo:

- Una orden debe tener al menos un producto.
- Una orden cancelada no puede confirmarse.
- Una orden confirmada no puede cancelarse.
- Una orden no puede confirmarse dos veces.
- Una orden no puede cancelarse dos veces.

## Relación con Infrastructure

La persistencia real está implementada en:

```text
OrderRepository
```

Ese repositorio implementa:

```text
IOrderRepository
```

y usa:

```text
AppDbContext
```

para guardar órdenes e items en PostgreSQL.

## Flujo de creación de orden

```text
Swagger / Cliente HTTP
        ↓
OrdersController
        ↓
IOrderService
        ↓
OrderService
        ↓
ICartRepository
        ↓
Cart
        ↓
Order
        ↓
IOrderRepository
        ↓
OrderRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

## Manejo de errores

Los errores de negocio son lanzados como `DomainException`.

El middleware global los convierte en respuestas HTTP claras.

Ejemplo al intentar crear una orden desde un carrito vacío:

```http
400 Bad Request
```

```json
{
	"message": "No se puede crear una orden desde un carrito vacío."
}
```

Ejemplo al intentar cancelar una orden confirmada:

```http
400 Bad Request
```

```json
{
	"message": "No se puede cancelar una orden confirmada."
}
```

## Pruebas realizadas

Se probaron desde Swagger:

- Crear orden desde carrito.
- Obtener orden por Id.
- Listar órdenes.
- Confirmar orden.
- Cancelar orden.
- Validar errores de dominio.

## Resultado

Al finalizar este entregable, el sistema permite operar órdenes de compra mediante HTTP.

El flujo de checkout empieza a estar disponible desde la API.
