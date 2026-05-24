# ADR 0011 - Integrar carrito con catálogo real

## Estado

Aceptado.

## Contexto

El carrito permitía agregar items enviando desde el cliente el `productName`, `unitPrice`, `productId` y `quantity`.

Esto funcionaba para un prototipo inicial, pero no es seguro para un sistema real.

El cliente no debería poder decidir el precio ni el nombre del producto.

## Decisión

El cliente enviará solamente:

- `ProductId`
- `Quantity`

El backend buscará el producto real usando `IProductRepository`.

A partir del producto encontrado se obtendrán:

- Nombre.
- Precio.
- Stock.
- Estado activo/inactivo.

## Motivos

- Evitar manipulación de precios desde el cliente.
- Evitar inconsistencias entre carrito y catálogo.
- Validar que el producto exista.
- Validar que el producto esté activo.
- Validar stock suficiente.
- Hacer que el backend sea la fuente de verdad.

## Consecuencias

- `CartService` ahora depende de `IProductRepository`.
- Los tests de Application necesitan un fake de productos.
- El request para agregar items al carrito es más simple.
- El flujo es más seguro y cercano a un e-commerce real.
