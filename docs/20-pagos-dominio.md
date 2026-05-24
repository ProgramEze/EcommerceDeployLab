# Pagos - Dominio

## Objetivo

En este entregable se creó el dominio inicial de pagos.

El objetivo fue modelar los pagos asociados a órdenes, sus métodos, estados y reglas de negocio principales.

## Archivos creados

Se crearon:

```text
backend/src/Ecommerce.Domain/Entities/Payment.cs
backend/src/Ecommerce.Domain/Enums/PaymentStatus.cs
backend/src/Ecommerce.Domain/Enums/PaymentMethod.cs
```

También se crearon tests en:

```text
backend/tests/Ecommerce.Tests/Domain/PaymentTests.cs
```

## Payment

`Payment` representa un intento de pago asociado a una orden.

Contiene:

- Id.
- OrderId.
- Amount.
- Method.
- Status.
- ExternalReference.
- CreatedAt.
- ApprovedAt.
- RejectedAt.
- CancelledAt.

## PaymentStatus

`PaymentStatus` representa el estado actual de un pago.

Estados iniciales:

```text
Pending
Approved
Rejected
Cancelled
```

## PaymentMethod

`PaymentMethod` representa el método elegido para pagar.

Métodos iniciales:

```text
CreditCard
DebitCard
BankTransfer
Cash
MercadoPago
```

## Reglas de negocio

El dominio de pagos protege las siguientes reglas:

- El identificador de la orden es obligatorio.
- El importe del pago debe ser mayor que cero.
- El método de pago debe ser válido.
- Un pago aprobado no puede rechazarse.
- Un pago aprobado no puede cancelarse.
- Un pago rechazado no puede aprobarse.
- Un pago rechazado no puede cancelarse.
- Un pago cancelado no puede aprobarse.
- Un pago cancelado no puede rechazarse.
- Un pago no puede aprobarse dos veces.
- Un pago no puede rechazarse dos veces.
- Un pago no puede cancelarse dos veces.

## ExternalReference

`ExternalReference` permite guardar una referencia externa del pago.

Más adelante podría representar un identificador de un proveedor de pago, como Mercado Pago, Stripe u otro procesador.

En esta etapa todavía no se integra ningún proveedor real.

## Transiciones de estado

El flujo inicial esperado es:

```text
Pending → Approved
Pending → Rejected
Pending → Cancelled
```

No se permiten transiciones desde estados finales hacia otros estados.

## Independencia del dominio

El dominio de pagos no depende de:

- ASP.NET Core.
- Entity Framework Core.
- PostgreSQL.
- Swagger.
- Proveedores externos de pago.

Esto permite probar sus reglas de negocio de forma aislada.

## Tests

Se crearon tests para validar:

- Crear pagos válidos.
- Evitar pagos sin orden.
- Evitar pagos con importe inválido.
- Evitar métodos de pago inválidos.
- Aprobar pagos.
- Rechazar pagos.
- Cancelar pagos.
- Evitar transiciones inválidas.

## Resultado

Al finalizar este entregable, el sistema cuenta con el dominio inicial necesario para representar pagos asociados a órdenes.

Todavía no se conecta con Application, Infrastructure ni API. Eso se implementará en entregables posteriores.
