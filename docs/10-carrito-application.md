# Carrito de compras - Application

## Objetivo

En este entregable se creó la capa Application para el carrito de compras.

El objetivo fue definir los casos de uso necesarios para operar con carritos sin depender todavía de API, Entity Framework Core ni PostgreSQL.

## Archivos creados

Se crearon DTOs, interfaces y servicios relacionados con carrito.

## DTOs creados

backend/src/Ecommerce.Application/DTOs/CartDto.cs
backend/src/Ecommerce.Application/DTOs/CartItemDto.cs
backend/src/Ecommerce.Application/DTOs/AddCartItemDto.cs
backend/src/Ecommerce.Application/DTOs/UpdateCartItemQuantityDto.cs

## Interfaces creadas

backend/src/Ecommerce.Application/Interfaces/ICartRepository.cs
backend/src/Ecommerce.Application/Interfaces/ICartService.cs

## Servicio creado

backend/src/Ecommerce.Application/Services/CartService.cs
