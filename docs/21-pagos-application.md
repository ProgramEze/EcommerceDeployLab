# Pagos - Application

## Objetivo

En este entregable se creó la capa Application para pagos.

El objetivo fue definir los casos de uso necesarios para registrar y administrar pagos asociados a órdenes.

## Archivos creados

Se crearon DTOs, interfaces y servicios relacionados con pagos.

## DTOs creados

```text
backend/src/Ecommerce.Application/DTOs/PaymentDto.cs
backend/src/Ecommerce.Application/DTOs/CreatePaymentDto.cs
```

## Interfaces creadas

```text
backend/src/Ecommerce.Application/Interfaces/IPaymentRepository.cs
backend/src/Ecommerce.Application/Interfaces/IPaymentService.cs
```

## Servicio creado

```text
backend/src/Ecommerce.Application/Services/PaymentService.cs
```

## Responsabilidad de PaymentService

`PaymentService` coordina los casos de uso relacionados con pagos.

Permite:

- Registrar un pago asociado a una orden.
- Obtener un pago por Id.
- Listar pagos.
- Listar pagos por orden.
- Aprobar un pago.
- Rechazar un pago.
- Cancelar un pago.

## Crear pago desde orden

Para crear un pago, el servicio busca la orden por Id.

Si la orden no existe, lanza una `DomainException`.

Si la orden no está confirmada, también lanza una `DomainException`.

Si la orden es válida, el pago se crea usando el total real de la orden.

## Por qué el cliente no envía el importe

`CreatePaymentDto` no incluye `Amount`.

El importe se obtiene desde la orden real.

Esto evita que el cliente pueda manipular el monto a pagar.

## Estados del pago

Desde Application se pueden ejecutar operaciones para:

- Aprobar pago.
- Rechazar pago.
- Cancelar pago.

Las reglas de transición siguen estando en el dominio, dentro de `Payment`.

## Flujo de creación de pago

```text
CreatePaymentDto
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
```

## Tests

Se crearon tests en:

```text
backend/tests/Ecommerce.Tests/Application/PaymentServiceTests.cs
```

Los tests verifican:

- Crear un pago desde una orden confirmada.
- Evitar crear un pago desde una orden inexistente.
- Evitar crear un pago desde una orden no confirmada.
- Obtener pagos por Id.
- Listar pagos.
- Listar pagos por orden.
- Aprobar pagos.
- Rechazar pagos.
- Cancelar pagos.

## Resultado

Al finalizar este entregable, el sistema cuenta con casos de uso de pagos listos para conectarse con Infrastructure y API.
