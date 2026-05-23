# ADR 0007 - Diseño del dominio del carrito

## Estado

Aceptado.

## Contexto

El sistema necesita permitir que un usuario seleccione productos antes de realizar una compra.

Esta selección temporal se representa mediante un carrito de compras.

El carrito debe manejar productos, cantidades y totales, respetando reglas de negocio.

## Decisión

Se modelará el carrito mediante dos entidades de dominio:

- `Cart`
- `CartItem`

`Cart` será la entidad principal y contendrá una colección de `CartItem`.

`CartItem` guardará una copia del nombre del producto y del precio unitario al momento de ser agregado.

## Motivos

- Separar la lógica del carrito de la API y la base de datos.
- Mantener las reglas de negocio dentro del dominio.
- Evitar que la colección de items sea modificada directamente desde afuera.
- Permitir calcular totales dentro del dominio.
- Preparar el modelo para una futura conversión de carrito a orden.
- Conservar una referencia del precio y nombre del producto al momento de agregarlo.

## Consecuencias

- `CartItem` duplica algunos datos del producto, como nombre y precio.
- Esta duplicación es intencional para conservar una foto del producto en el carrito.
- Más adelante se deberá mapear la relación entre carrito, items y productos en Infrastructure.
- El dominio del carrito podrá probarse sin depender de base de datos ni API.
