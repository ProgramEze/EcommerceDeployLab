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
- Carritos con cantidades inválidas.
- Items con precio unitario inválido.

## Decisión

Las entidades del dominio usarán propiedades con `private set` y métodos públicos para modificar su estado.

Ejemplo:

```csharp
product.UpdateDetails(...);
product.IncreaseStock(...);
product.DecreaseStock(...);
```

En lugar de permitir modificaciones directas como:

```csharp
product.Price = -100;
product.Stock = -5;
```

También se protegerán colecciones internas usando listas privadas y colecciones públicas de solo lectura.

Ejemplo:

```csharp
private readonly List<CartItem> _items = new();

public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
```

## Motivos

- Proteger las reglas del negocio.
- Evitar estados inválidos.
- Hacer que el código sea más expresivo.
- Facilitar los tests unitarios.
- Mantener el dominio independiente de infraestructura y API.
- Evitar que otras capas modifiquen entidades saltándose validaciones.
- Centralizar la lógica de negocio dentro de las entidades correspondientes.

## Consecuencias

- Se escribe más código dentro de las entidades.
- Los cambios deben realizarse mediante métodos específicos.
- No se puede modificar libremente el estado desde cualquier capa.
- Las pruebas unitarias deben validar los métodos del dominio.
- El dominio queda más robusto y fácil de mantener.
