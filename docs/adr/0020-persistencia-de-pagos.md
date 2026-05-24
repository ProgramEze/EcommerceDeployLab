# ADR 0020 - Persistencia de pagos con Entity Framework Core

## Estado

Aceptado.

## Contexto

El dominio de pagos ya existe mediante la entidad `Payment`.

La capa Application ya define los casos de uso y el contrato `IPaymentRepository`.

Ahora es necesario persistir pagos reales en PostgreSQL.

## Decisión

Se implementará `IPaymentRepository` en Infrastructure usando Entity Framework Core y PostgreSQL.

La entidad `Payment` será persistida como tabla `Payments`.

## Motivos

- Mantener Application independiente de Entity Framework Core.
- Respetar el contrato `IPaymentRepository`.
- Persistir pagos reales en PostgreSQL.
- Permitir consultar pagos por orden.
- Preparar la API de pagos para el siguiente entregable.
- Preparar el sistema para una futura integración con proveedores externos.

## Decisión sobre relación con Order

`Payment` mantiene una referencia a la orden mediante `OrderId`.

No se agrega una navegación directa hacia `Order` en el dominio.

Esto permite mantener el dominio simple y evita introducir dependencias innecesarias.

## Consecuencias

- Infrastructure configura explícitamente cómo se persiste `Payment`.
- Los enums de pago se almacenan como enteros.
- Las migraciones deberán crear la tabla `Payments`.
- El dominio mantiene su independencia de detalles de base de datos.
