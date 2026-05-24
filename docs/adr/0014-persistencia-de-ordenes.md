# ADR 0014 - Persistencia de órdenes con Entity Framework Core

## Estado

Aceptado.

## Contexto

El dominio de órdenes ya existe mediante las entidades `Order` y `OrderItem`.

La capa Application ya define los casos de uso y el contrato `IOrderRepository`.

Ahora es necesario persistir órdenes reales en PostgreSQL.

## Decisión

Se implementará `IOrderRepository` en Infrastructure usando Entity Framework Core y PostgreSQL.

La entidad `Order` será persistida como tabla `Orders`.

Los items de la orden serán persistidos como tabla `OrderItems`.

## Motivos

- Mantener Application independiente de Entity Framework Core.
- Respetar el contrato `IOrderRepository`.
- Persistir órdenes reales en PostgreSQL.
- Mantener la colección de items protegida en el dominio.
- Permitir reconstruir una orden completa con sus items.
- Preservar datos históricos de productos en `OrderItem`.
- Preparar la API de órdenes para el siguiente entregable.

## Decisión sobre OrderItem

`OrderItem` se persistirá en una tabla separada llamada `OrderItems`.

La relación con `Order` se manejará mediante una clave foránea `OrderId`.

`OrderId` será configurado como shadow property para no contaminar el dominio con detalles de infraestructura.

## Consecuencias

- Infrastructure necesita configurar explícitamente la relación entre `Order` y `OrderItem`.
- Entity Framework Core debe usar backing fields para cargar la colección protegida.
- Las migraciones deberán crear las tablas `Orders` y `OrderItems`.
- El dominio mantiene su independencia de detalles de base de datos.
