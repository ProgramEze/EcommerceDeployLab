# ADR 0010 - Exponer carrito mediante controller

## Estado

Aceptado.

## Contexto

El sistema ya cuenta con dominio, casos de uso y persistencia para el carrito de compras.

Ahora es necesario permitir que clientes externos, como Swagger, Postman o el futuro frontend Angular, puedan interactuar con el carrito mediante HTTP.

El carrito es una pieza central del e-commerce, porque representa la selección temporal de productos antes de la compra.

## Decisión

Se expondrán los casos de uso del carrito mediante un controller llamado `CartsController`.

El controller usará la interfaz `ICartService` para comunicarse con la capa Application.

Los endpoints permitirán:

- Crear carrito.
- Obtener carrito por Id.
- Agregar items.
- Cambiar cantidades.
- Quitar items.
- Vaciar carrito.
- Eliminar carrito.

## Motivos

- Mantener la API separada de la lógica de negocio.
- Reutilizar los casos de uso ya definidos en Application.
- Evitar que el controller manipule directamente entidades del dominio.
- Mantener la persistencia oculta detrás de `ICartRepository`.
- Exponer endpoints claros para el flujo de carrito.
- Preparar el sistema para ser consumido por el futuro frontend Angular.

## Consecuencias

- Se agregan nuevos endpoints HTTP.
- La API empieza a cubrir el flujo real del e-commerce.
- El frontend podrá consumir operaciones de carrito más adelante.
- Los errores de dominio seguirán siendo manejados por el middleware global.
- El carrito queda expuesto como recurso principal de la API.
