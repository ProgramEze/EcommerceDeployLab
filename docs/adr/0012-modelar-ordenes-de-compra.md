# ADR 0012 - Modelar órdenes de compra en el dominio

## Estado

Aceptado.

## Contexto

El sistema ya permite crear carritos y agregar productos.

El siguiente paso del flujo e-commerce es convertir un carrito en una orden de compra.

Una orden debe representar una compra en un momento específico y conservar los datos históricos de los productos comprados.

## Decisión

Se crearán las entidades de dominio:

- `Order`
- `OrderItem`

También se creará el enum:

- `OrderStatus`

La orden iniciará en estado `Pending`.

## Motivos

- Representar el resultado del checkout.
- Separar el carrito de la compra final.
- Conservar precios históricos.
- Conservar nombres históricos de productos.
- Controlar estados válidos mediante un enum.
- Preparar el sistema para pagos y confirmación de compra.

## Consecuencias

- `OrderItem` duplicará datos del producto, como nombre y precio.
- Esta duplicación es intencional para conservar una foto histórica de la compra.
- Más adelante habrá que implementar Application, Infrastructure y API para órdenes.
- El dominio podrá validar transiciones de estado antes de integrar pagos.
