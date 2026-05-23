# Manejo global de errores y validaciones de API

## Objetivo

En este entregable se implementó un middleware global para manejar errores de forma centralizada.

El objetivo es evitar que la API devuelva errores genéricos o poco claros cuando ocurre una excepción controlada del dominio.

## Problema detectado

Antes de este entregable, si se enviaban datos inválidos al crear o actualizar productos, el dominio podía lanzar una `DomainException`.

Por ejemplo:

- Precio igual a cero.
- Stock negativo.
- Nombre vacío.
- Descripción vacía.

Sin un manejo adecuado, estas excepciones podían convertirse en respuestas HTTP `500 Internal Server Error`.

Esto no era correcto, porque esos errores no representan una falla inesperada del servidor, sino una validación de negocio.

## Solución implementada

Se creó el middleware:

`backend/src/Ecommerce.Api/Middlewares/ExceptionHandlingMiddleware.cs`

Este middleware intercepta excepciones producidas durante la ejecución de la request.

## Manejo de DomainException

Cuando se captura una `DomainException`, la API devuelve:

```http
400 Bad Request
```
