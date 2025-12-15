# InventorySystem - Evaluación Técnica Backend .NET

Solución de ejemplo con arquitectura de **microservicios** y estilo **hexagonal**, compuesta por:

- `ProductService` (Gestión de Productos)
- `TransactionService` (Gestión de Transacciones)
- Script `database.sql` para crear el esquema en SQL Server.

## Requisitos

- .NET 8 SDK
- SQL Server local (o alguna instancia accesible)
- Visual Studio 2022 o VS Code

## Configuración de base de datos

1. Crear una base de datos, por ejemplo `prueba_netby`.
2. Ejecutar el script `database.sql` en esa base de datos.

## Cómo ejecutar los microservicios

1. Abrir `InventorySystem.sln` en Visual Studio.
2. Establecer como proyecto de inicio (StartUp Project) `ProductService.Api`.
3. Ajustar la cadena de conexión en `ProductService/ProductService.Api/appsettings.json` y `TransactionService/TransactionService.Api/appsettings.json` si es necesario.
4. Ejecutar `ProductService.Api` (por defecto lanzará Swagger en un puerto HTTPS).
5. Ejecutar `TransactionService.Api` (otro puerto).

Asegúrate de que el `BaseUrl` de `ProductService` en `TransactionService/ProductService.Api/appsettings.json` apunte a la URL real donde se está ejecutando `ProductService`.
# InventorySystem - Evaluación Técnica Backend .NET

Solución de ejemplo con arquitectura de **microservicios** y estilo **hexagonal**, compuesta por:

- `ProductService` (Gestión de Productos)
- `TransactionService` (Gestión de Transacciones)
- Script `database.sql` para crear el esquema en SQL Server.

## Requisitos

- .NET 8 SDK
- SQL Server local (o alguna instancia accesible)
- Visual Studio 2022 o VS Code

## Configuración de base de datos

1. Crear una base de datos, por ejemplo `prueba_netby`.
2. Ejecutar el script `database.sql` en esa base de datos.

## Cómo ejecutar los microservicios

1. Abrir `InventorySystem.sln` en Visual Studio.
2. Establecer como proyecto de inicio (StartUp Project) `ProductService.Api`.
3. Ajustar la cadena de conexión en `ProductService/ProductService.Api/appsettings.json` y `TransactionService/TransactionService.Api/appsettings.json` si es necesario.
4. Ejecutar `ProductService.Api` (por defecto lanzará Swagger en un puerto HTTPS).
5. Ejecutar `TransactionService.Api` (otro puerto).

Asegúrate de que el `BaseUrl` de `ProductService` en `TransactionService/ProductService.Api/appsettings.json` apunte a la URL real donde se está ejecutando `ProductService`.

## Endpoints principales

### ProductService

- `GET /api/products`  
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `POST /api/products/{id}/increase-stock?quantity=Q`
- `POST /api/products/{id}/decrease-stock?quantity=Q`
- `GET /api/products/search?name=ABC`  
  Usa una lista ordenada en memoria + **búsqueda binaria** para soportar muchas consultas de lectura.

### TransactionService

CRUD de transacciones

GET /api/transactions

GET /api/transactions/{id}

POST /api/transactions

PUT /api/transactions/{id}

DELETE /api/transactions/{id}

Historial filtrado

GET /api/transactions/history?productId=...&from=...&to=...&type=...

## Notas de arquitectura

/Domain
    - Entities
    - Ports (interfaces)
/Application
    - Commands
    - Handlers (casos de uso)
/Infrastructure
    - EF Core Repositories
    - DbContext
    - Http Clients
/Api
    - Controllers REST

Crear un producto
{
  "name": "Mouse Gamer RGB",
  "description": "7200 DPI",
  "category": "Periféricos",
  "imageUrl": "http://example.com/mouse.png",
  "price": 29.99,
  "initialStock": 50
}

Registrar compra
{
  "productId": "ID_DEL_PRODUCTO",
  "type": 1,
  "quantity": 20,
  "unitPrice": 25.50,
  "detail": "Compra inicial"
}

Registrar venta
{
  "productId": "ID_DEL_PRODUCTO",
  "type": 2,
  "quantity": 5,
  "unitPrice": 35.99,
  "detail": "Venta al cliente final"
}



