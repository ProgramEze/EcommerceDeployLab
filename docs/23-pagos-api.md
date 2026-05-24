# Pagos - API

## Objetivo

En este entregable se expusieron endpoints HTTP para operar con pagos.

El objetivo fue conectar la capa `Ecommerce.Api` con los casos de uso de pagos definidos en `Ecommerce.Application` y con la persistencia real implementada en `Ecommerce.Infrastructure`.

Con esto, el sistema permite registrar pagos asociados a órdenes confirmadas, consultar pagos y modificar su estado.

## Controlador creado

Se creó el controlador:

```text
backend/src/Ecommerce.Api/Controllers/PaymentsController.cs
```

Este controlador usa la interfaz:

```text
IPaymentService
```

para acceder a los casos de uso de pagos.

## Endpoints creados

```http
GET    /api/payments
GET    /api/payments/{id}
GET    /api/payments/order/{orderId}
POST   /api/payments
PATCH  /api/payments/{id}/approve
PATCH  /api/payments/{id}/reject
PATCH  /api/payments/{id}/cancel
```

## GET /api/payments

Devuelve todos los pagos registrados.

Ejemplo:

```http
GET /api/payments
```

Respuesta esperada:

```http
200 OK
```

## GET /api/payments/{id}

Devuelve un pago por Id.

Ejemplo:

```http
GET /api/payments/{id}
```

Si el pago existe, devuelve:

```http
200 OK
```

Si el pago no existe, devuelve:

```http
404 Not Found
```

Con una respuesta como:

```json
{
	"message": "Pago no encontrado."
}
```

## GET /api/payments/order/{orderId}

Devuelve los pagos asociados a una orden.

Ejemplo:

```http
GET /api/payments/order/{orderId}
```

Devuelve una lista de pagos.

## POST /api/payments

Registra un pago para una orden confirmada.

Body esperado:

```json
{
	"orderId": "guid-de-la-orden",
	"method": 1,
	"externalReference": "POSTMAN-PAYMENT-001"
}
```

## Métodos de pago

El enum `PaymentMethod` se representa inicialmente con valores numéricos:

```text
1 = CreditCard
2 = DebitCard
3 = BankTransfer
4 = Cash
5 = MercadoPago
```

## Estados de pago

El enum `PaymentStatus` se representa inicialmente con valores numéricos:

```text
1 = Pending
2 = Approved
3 = Rejected
4 = Cancelled
```

## Por qué el cliente no envía el importe

El request de creación de pago no incluye `amount`.

El importe se obtiene desde la orden real.

Esto evita que el cliente pueda manipular el monto a pagar.

## PATCH /api/payments/{id}/approve

Aprueba un pago pendiente.

Ejemplo:

```http
PATCH /api/payments/{id}/approve
```

Si el pago existe y está pendiente, devuelve el pago con estado `Approved`.

## PATCH /api/payments/{id}/reject

Rechaza un pago pendiente.

Ejemplo:

```http
PATCH /api/payments/{id}/reject
```

Si el pago existe y está pendiente, devuelve el pago con estado `Rejected`.

## PATCH /api/payments/{id}/cancel

Cancela un pago pendiente.

Ejemplo:

```http
PATCH /api/payments/{id}/cancel
```

Si el pago existe y está pendiente, devuelve el pago con estado `Cancelled`.

## Reglas de negocio

Las reglas principales viven en el dominio, dentro de `Payment`.

Por ejemplo:

- Un pago aprobado no puede rechazarse.
- Un pago aprobado no puede cancelarse.
- Un pago rechazado no puede aprobarse.
- Un pago cancelado no puede aprobarse.
- Un pago no puede aprobarse dos veces.
- Un pago no puede rechazarse dos veces.
- Un pago no puede cancelarse dos veces.

## Relación con Application

`PaymentsController` no manipula directamente entidades del dominio ni usa Entity Framework Core.

El controller llama a:

```text
IPaymentService
```

La implementación concreta es:

```text
PaymentService
```

## Relación con Infrastructure

La persistencia real está implementada en:

```text
PaymentRepository
```

Ese repositorio implementa:

```text
IPaymentRepository
```

y usa:

```text
AppDbContext
```

para guardar pagos en PostgreSQL.

## Flujo de creación de pago

```text
Postman / Cliente HTTP
        ↓
PaymentsController
        ↓
IPaymentService
        ↓
PaymentService
        ↓
IOrderRepository
        ↓
Order
        ↓
Payment
        ↓
IPaymentRepository
        ↓
PaymentRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

## Manejo de errores

Los errores de negocio son lanzados como `DomainException`.

El middleware global los convierte en respuestas HTTP claras.

Ejemplo al intentar crear un pago para una orden no confirmada:

```http
400 Bad Request
```

```json
{
	"message": "Solo se puede registrar un pago para una orden confirmada."
}
```

Ejemplo al intentar rechazar un pago aprobado:

```http
400 Bad Request
```

```json
{
	"message": "No se puede rechazar un pago aprobado."
}
```

## Pruebas realizadas

Se probaron desde Postman:

- Registrar pago.
- Obtener pago por Id.
- Listar pagos.
- Listar pagos por orden.
- Aprobar pago.
- Rechazar pago.
- Cancelar pago.
- Validar errores de dominio.

## Resultado

Al finalizar este entregable, el sistema permite operar pagos mediante HTTP.

El flujo de cobro empieza a estar disponible desde la API.
