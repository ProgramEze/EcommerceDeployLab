# ADR 0002 - Proteger entidades de dominio

## Estado

Aceptado.

## Contexto

El sistema necesita representar reglas de negocio del e-commerce de forma segura.

Si las propiedades de una entidad pudieran modificarse libremente desde cualquier parte del sistema, sería fácil crear estados inválidos, por ejemplo:

- Productos sin nombre.
- Productos con precio negativo.
- Stock menor que cero.
- Descuento de más stock del disponible.

## Decisión

Las entidades del dominio usarán propiedades con `private set` y métodos públicos para modificar su estado.

Ejemplo:

```csharp
product.UpdateDetails(...);
product.IncreaseStock(...);
product.DecreaseStock(...);