# ADR 0021 - Exponer pagos mediante controller

## Estado

Aceptado.

## Contexto

El sistema ya cuenta con dominio, casos de uso y persistencia para pagos.

Ahora es necesario permitir que clientes externos, como Postman o el futuro frontend Angular, puedan registrar y administrar pagos mediante HTTP.

Los pagos representan una parte central del flujo de checkout y permiten simular el cobro de una orden.

## Decisión

Se expondrán los casos de uso de pagos mediante un controller llamado `PaymentsController`.

El controller usará la interfaz `IPaymentService` para comunicarse con la capa Application.

Los endpoints permitirán:

- Listar pagos.
- Obtener un pago por Id.
- Obtener pagos por orden.
- Registrar un pago.
- Aprobar un pago.
- Rechazar un pago.
- Cancelar un pago.

## Motivos

- Mantener la API separada de la lógica de negocio.
- Reutilizar los casos de uso ya definidos en Application.
- Evitar que el controller manipule directamente entidades del dominio.
- Mantener la persistencia oculta detrás de `IPaymentRepository`.
- Exponer endpoints claros para el flujo de pagos.
- Preparar el sistema para la futura integración con proveedores externos.

## Consecuencias

- Se agregan nuevos endpoints HTTP.
- La API empieza a cubrir el flujo de cobro.
- El frontend podrá consumir operaciones de pagos más adelante.
- Los errores de dominio seguirán siendo manejados por el middleware global.
- Todavía queda pendiente integrar pagos con el estado final de órdenes.
