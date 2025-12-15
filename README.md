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

1. Crear una base de datos, por ejemplo `InventoryDb`.
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

- `POST /api/transactions`  
  Registra compras/ventas, valida stock llamando a `ProductService` y luego ajusta el stock.
- `GET /api/transactions/history?productId=...&from=...&to=...&type=...`  
  Devuelve el historial de transacciones filtrando por producto, fecha y tipo.

## Notas de arquitectura

- Cada microservicio se divide en:
  - `Domain` (entidades y puertos)
  - `Application` (casos de uso / handlers)
  - `Infrastructure` (EF Core, repositorios, adaptadores externos)
  - `Api` (endpoints HTTP)
- La comunicación entre microservicios es síncrona usando HTTP (`HttpClient`).
- Para búsquedas por nombre de producto se utilizan:
  - Índices en SQL Server (`IX_Products_Name`).
  - Caché en memoria de productos ordenados por nombre + búsqueda binaria para millones de consultas de solo lectura.
