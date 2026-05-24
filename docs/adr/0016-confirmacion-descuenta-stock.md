# ADR 0016 - Descontar stock al confirmar una orden

## Estado

Aceptado.

## Contexto

El sistema ya permite crear órdenes desde carritos reales y confirmarlas.

Sin embargo, confirmar una orden no modificaba el stock de los productos.

En un e-commerce real, una orden confirmada debe afectar el inventario.

## Decisión

Al confirmar una orden, el sistema descontará del stock real del catálogo las cantidades incluidas en la orden.

La operación se realizará desde `OrderService`, usando `IProductRepository`.

## Motivos

- Reflejar correctamente el flujo de compra.
- Mantener el catálogo sincronizado con las órdenes confirmadas.
- Evitar vender más unidades de las disponibles.
- Centralizar el caso de uso en Application.
- Reutilizar las reglas del dominio de `Product`.

## Validaciones

Antes de descontar stock, se validará:

- Que los productos existan.
- Que los productos estén activos.
- Que exista stock suficiente.
- Que la orden no haya sido confirmada previamente.
- Que la orden no esté cancelada.

## Consecuencias

- `OrderService` ahora depende de `IProductRepository`.
- La confirmación de una orden modifica productos.
- Los tests de Application necesitan simular productos.
- Más adelante será importante considerar transacciones para garantizar consistencia total.
- Es importante validar el estado de la orden antes de modificar el stock para evitar descuentos duplicados.
