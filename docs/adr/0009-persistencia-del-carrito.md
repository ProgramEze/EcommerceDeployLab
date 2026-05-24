# ADR 0009 - Persistencia del carrito con Entity Framework Core

## Estado

Aceptado.

## Contexto

El dominio del carrito ya existe mediante las entidades `Cart` y `CartItem`.

La capa Application ya define los casos de uso y el contrato `ICartRepository`.

Ahora es necesario persistir carritos reales en PostgreSQL.

## Decisión

Se implementará `ICartRepository` en Infrastructure usando Entity Framework Core y PostgreSQL.

La entidad `Cart` será persistida como tabla `Carts`.

Los items del carrito serán persistidos como tabla `CartItems`.

## Motivos

- Mantener Application independiente de Entity Framework Core.
- Respetar el contrato `ICartRepository`.
- Persistir carritos reales en PostgreSQL.
- Mantener la colección de items protegida en el dominio.
- Permitir reconstruir un carrito completo con sus items.
- Preparar la API del carrito para el siguiente entregable.

## Decisión sobre CartItem

`CartItem` se persistirá en una tabla separada llamada `CartItems`.

La relación con `Cart` se manejará mediante una clave foránea `CartId`.

`CartId` será configurado como shadow property para no contaminar el dominio con detalles de infraestructura.

## Consecuencias

- Infrastructure necesita configurar explícitamente la relación entre `Cart` y `CartItem`.
- Entity Framework Core debe usar backing fields para cargar la colección protegida.
- Las migraciones deberán crear las tablas `Carts` y `CartItems`.
- El dominio mantiene su independencia de detalles de base de datos.
