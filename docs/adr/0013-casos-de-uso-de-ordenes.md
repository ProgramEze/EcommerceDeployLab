# ADR 0013 - Definir casos de uso de órdenes en Application

## Estado

Aceptado.

## Contexto

El dominio de órdenes ya permite representar una compra y sus items.

Ahora el sistema necesita coordinar operaciones de aplicación para crear órdenes a partir de carritos reales.

## Decisión

Se creará un servicio de aplicación llamado `OrderService` y una interfaz `IOrderService`.

También se definirá `IOrderRepository` como contrato de persistencia.

## Motivos

- Separar los casos de uso de la API.
- Evitar que la API manipule directamente entidades del dominio.
- Reutilizar el carrito como origen del checkout.
- Mantener las reglas de negocio en el dominio.
- Preparar el sistema para persistencia y API de órdenes.

## Consecuencias

- Se agregan DTOs para órdenes.
- Se requiere mapear entre entidades y DTOs.
- Infrastructure deberá implementar `IOrderRepository`.
- El checkout empieza a tomar forma como caso de uso real.
