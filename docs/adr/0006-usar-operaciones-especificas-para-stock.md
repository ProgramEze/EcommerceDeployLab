# ADR 0006 - Usar operaciones específicas para modificar stock

## Estado

Aceptado.

## Contexto

El stock de un producto no es un dato común. Representa disponibilidad real para ventas y debe respetar reglas de negocio.

Modificar stock directamente dentro de una actualización general del producto puede generar errores y estados inconsistentes.

## Decisión

El stock se modificará mediante operaciones específicas.

Se usarán endpoints dedicados para:

- Incrementar stock.
- Descontar stock.

## Motivos

- Evitar modificaciones accidentales del stock.
- Mantener reglas de negocio concentradas en el dominio.
- Preparar el sistema para futuras operaciones como compras, cancelaciones y devoluciones.
- Hacer la API más expresiva.
- Evitar que `UpdateProductDto` mezcle datos descriptivos con inventario.

## Consecuencias

- Se agregan endpoints adicionales.
- El flujo de actualización de producto queda más explícito.
- El stock queda mejor protegido.
