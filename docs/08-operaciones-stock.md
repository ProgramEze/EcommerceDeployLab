# Operaciones específicas de stock

## Objetivo

En este entregable se agregaron operaciones específicas para incrementar y descontar stock de productos.

El objetivo fue evitar que el stock se modifique como un dato común dentro de la actualización general del producto.

## Endpoints creados

Se agregaron los siguientes endpoints:

```http
PATCH /api/products/{id}/stock/increase
PATCH /api/products/{id}/stock/decrease
```
