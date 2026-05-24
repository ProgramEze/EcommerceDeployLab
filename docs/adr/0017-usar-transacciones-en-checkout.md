# ADR 0017 - Usar transacciones en checkout

## Estado

Aceptado.

## Contexto

La confirmación de una orden realiza varias operaciones que deben mantenerse consistentes:

- Validar la orden.
- Validar productos.
- Descontar stock.
- Confirmar la orden.

Si una parte del proceso falla, no debería guardarse solo una parte de los cambios.

## Decisión

Se usará una transacción para envolver el flujo de confirmación de órdenes.

Para no acoplar Application a Entity Framework Core, se definirá una interfaz `IUnitOfWork` en Application.

Infrastructure implementará esa interfaz mediante `EfUnitOfWork`.

## Motivos

- Evitar estados inconsistentes.
- Asegurar atomicidad en el checkout.
- Mantener Application independiente de EF Core.
- Preparar el sistema para flujos más reales de compra.
- Permitir rollback si ocurre un error durante la confirmación.

## Consecuencias

- `OrderService` ahora depende de `IUnitOfWork`.
- Infrastructure debe implementar la transacción concreta.
- Los tests unitarios requieren un fake de `IUnitOfWork`.
- El checkout queda mejor preparado para escenarios productivos.
