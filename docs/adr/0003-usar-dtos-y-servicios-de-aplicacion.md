# ADR 0003 - Usar DTOs y servicios de aplicación

## Estado

Aceptado.

## Contexto

El sistema necesita exponer funcionalidades como crear, listar, actualizar y eliminar productos.

Si la API trabajara directamente con entidades del dominio, se mezclarían responsabilidades y se expondría el modelo interno del sistema.

## Decisión

Usaremos DTOs y servicios de aplicación.

Los DTOs representarán los datos que entran y salen de la aplicación.

Los servicios de aplicación coordinarán los casos de uso principales.

## Ejemplo

```csharp
public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
```

```csharp
public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto);
}
```

## Motivos

- Evitar exponer entidades del dominio directamente.
- Separar entrada/salida de datos del modelo interno.
- Facilitar cambios futuros en la API.
- Facilitar validaciones y mapeos.
- Mantener Application independiente de Infrastructure.

## Consecuencias

- Hay que escribir clases DTO adicionales.
- Hay que mapear entre entidades y DTOs.
- El sistema queda más mantenible y preparado para crecer.