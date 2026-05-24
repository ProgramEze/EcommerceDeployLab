# ADR 0019 - Definir casos de uso de pagos en Application

## Estado

Aceptado.

## Contexto

El dominio de pagos ya permite representar un pago, su método y sus estados.

Ahora el sistema necesita coordinar operaciones de aplicación para registrar pagos asociados a órdenes.

## Decisión

Se creará un servicio de aplicación llamado `PaymentService` y una interfaz `IPaymentService`.

También se definirá `IPaymentRepository` como contrato de persistencia.

## Motivos

- Separar los casos de uso de la API.
- Evitar que la API manipule directamente entidades del dominio.
- Usar la orden como fuente de verdad para el importe.
- Mantener las reglas de transición de estado dentro del dominio.
- Preparar el sistema para persistencia y API de pagos.

## Consecuencias

- Se agregan DTOs para pagos.
- Se requiere mapear entre entidades y DTOs.
- Infrastructure deberá implementar `IPaymentRepository`.
- El flujo de cobro empieza a tomar forma como caso de uso real.
