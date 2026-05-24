# ADR 0018 - Modelar pagos en el dominio

## Estado

Aceptado.

## Contexto

El sistema ya permite crear órdenes desde carritos, confirmar órdenes y descontar stock.

El siguiente paso del flujo e-commerce es representar pagos asociados a órdenes.

Aunque todavía no se integrará un proveedor real de pagos, el sistema necesita un modelo interno para representar intentos de pago, métodos y estados.

## Decisión

Se creará la entidad de dominio:

- `Payment`

También se crearán los enums:

- `PaymentStatus`
- `PaymentMethod`

El pago iniciará en estado `Pending`.

## Motivos

- Representar pagos asociados a órdenes.
- Preparar el sistema para simulación o integración futura de cobros.
- Evitar estados arbitrarios mediante enums.
- Modelar reglas de transición de estados.
- Mantener el concepto de pago independiente de infraestructura o proveedores externos.

## Consecuencias

- El dominio incorpora una nueva entidad.
- Más adelante habrá que crear Application, Infrastructure y API para pagos.
- La integración con proveedores externos deberá adaptarse al modelo interno.
- El sistema podrá simular pagos antes de integrar un procesador real.
