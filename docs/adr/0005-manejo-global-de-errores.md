# ADR 0005 - Manejo global de errores mediante middleware

## Estado

Aceptado.

## Contexto

La API puede recibir datos inválidos desde clientes externos como Swagger, Postman o el futuro frontend Angular.

Cuando una regla de negocio se incumple, el dominio lanza una `DomainException`.

Sin un manejo centralizado, estas excepciones pueden terminar devolviendo errores `500 Internal Server Error`, aunque en realidad representan errores esperados de validación de negocio.

## Decisión

Usaremos un middleware global para capturar excepciones y convertirlas en respuestas HTTP consistentes.

El middleware manejará inicialmente:

- `DomainException` como `400 Bad Request`.
- Excepciones inesperadas como `500 Internal Server Error`.

## Motivos

- Evitar repetir `try/catch` en todos los controladores.
- Centralizar el manejo de errores.
- Devolver respuestas claras al cliente.
- No exponer detalles técnicos internos.
- Preparar la API para un comportamiento más profesional.

## Consecuencias

- Todas las excepciones pasan por un punto central.
- Es importante registrar correctamente los errores inesperados.
- Más adelante se puede ampliar el middleware para manejar otros tipos de errores.
