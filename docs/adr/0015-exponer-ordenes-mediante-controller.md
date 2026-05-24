# ADR 0015 - Exponer órdenes mediante controller

## Estado

Aceptado.

## Contexto

El sistema ya cuenta con dominio, casos de uso y persistencia para órdenes de compra.

Ahora es necesario permitir que clientes externos, como Swagger, Postman o el futuro frontend Angular, puedan crear y consultar órdenes mediante HTTP.

Las órdenes representan el resultado del checkout y son una pieza central del flujo e-commerce.

## Decisión

Se expondrán los casos de uso de órdenes mediante un controller llamado `OrdersController`.

El controller usará la interfaz `IOrderService` para comunicarse con la capa Application.

Los endpoints permitirán:

- Listar órdenes.
- Obtener una orden por Id.
- Crear una orden desde un carrito.
- Confirmar una orden.
- Cancelar una orden.

## Motivos

- Mantener la API separada de la lógica de negocio.
- Reutilizar los casos de uso ya definidos en Application.
- Evitar que el controller manipule directamente entidades del dominio.
- Mantener la persistencia oculta detrás de `IOrderRepository`.
- Exponer endpoints claros para el flujo de checkout.
- Preparar el sistema para la futura integración de pagos.

## Consecuencias

- Se agregan nuevos endpoints HTTP.
- La API empieza a cubrir el flujo de checkout.
- El frontend podrá consumir operaciones de órdenes más adelante.
- Los errores de dominio seguirán siendo manejados por el middleware global.
- Todavía queda pendiente integrar pagos y descontar stock al confirmar la compra.
