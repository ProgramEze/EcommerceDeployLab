# ADR 0008 - Definir casos de uso del carrito en Application

## Estado

Aceptado.

## Contexto

El dominio del carrito ya contiene las reglas principales mediante `Cart` y `CartItem`.

Ahora el sistema necesita coordinar operaciones de aplicación para que futuras capas, como API, puedan interactuar con el carrito sin conocer detalles internos del dominio.

## Decisión

Se creará un servicio de aplicación llamado `CartService` y una interfaz `ICartService`.

También se definirá `ICartRepository` como contrato de persistencia.

## Motivos

- Separar los casos de uso de la API.
- Evitar que la API manipule directamente entidades del dominio.
- Mantener las reglas de negocio en el dominio.
- Permitir probar casos de uso sin base de datos real.
- Preparar el sistema para Infrastructure y API del carrito.

## Consecuencias

- Se agregan DTOs para entrada y salida de datos.
- Se requiere mapear entre entidades y DTOs.
- Infrastructure deberá implementar `ICartRepository`.
