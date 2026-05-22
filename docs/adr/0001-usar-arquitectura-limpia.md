# ADR 0001 - Usar arquitectura limpia en el backend

## Estado

Aceptado.

## Contexto

El proyecto busca simular un sistema real de e-commerce que pueda crecer en funcionalidades y ser desplegado en la nube.

Para evitar que la lógica de negocio quede mezclada con controladores, base de datos o infraestructura, se decidió separar el backend en capas.

## Decisión

Usaremos una estructura inspirada en Clean Architecture:

- Ecommerce.Domain
- Ecommerce.Application
- Ecommerce.Infrastructure
- Ecommerce.Api

## Motivos

- El dominio queda protegido de detalles técnicos.
- La lógica de negocio se puede probar más fácilmente.
- La infraestructura puede cambiar sin romper el núcleo del sistema.
- La API solo se encarga de exponer endpoints.
- El proyecto queda mejor preparado para crecer.

## Consecuencias

- Hay más proyectos y más estructura inicial.
- El desarrollo puede parecer más lento al comienzo.
- El mantenimiento a mediano y largo plazo será más ordenado.