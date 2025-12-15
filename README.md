# InventorySystem - Evaluación Técnica Backend .NET

Este repositorio contiene una solución de ejemplo con arquitectura de **microservicios** y estilo **hexagonal**. Incluye:

- `ProductService` (gestión de productos)
- `TransactionService` (gestión de transacciones)
- Script `database.sql` para crear el esquema en PostgreSQL

## Requisitos previos

- .NET 8 SDK
- PostgreSQL 14+ con acceso de superusuario o un usuario con permisos para crear tablas
- Herramienta de línea de comandos para PostgreSQL (`psql`) o un cliente gráfico
- Visual Studio 2022 o VS Code (opcional si prefieres interfaz gráfica)

## Estructura de la solución

- `InventorySystem.sln`: solución principal
- `ProductService/`
  - `ProductService.Api`: API HTTP (Swagger habilitado)
  - `ProductService.Application`: casos de uso
  - `ProductService.Domain`: entidades y contratos
  - `ProductService.Infrastructure`: acceso a datos (PostgreSQL via Npgsql)
- `TransactionService/`
  - `TransactionService.Api`: API HTTP (Swagger habilitado)
  - `TransactionService.Application`: casos de uso
  - `TransactionService.Domain`: entidades y contratos
  - `TransactionService.Infrastructure`: acceso a datos y cliente HTTP hacia ProductService

## Configuración de base de datos

1. Crear la base de datos, por ejemplo `prueba_netby`.
2. Ejecutar el script `database.sql` dentro de esa base de datos:
   ```bash
   psql -h <host> -U <usuario> -d prueba_netby -f database.sql
   ```
   El script crea las tablas `products` y `stock_transactions` e índices básicos.

## Configuración de los servicios

Ambos microservicios usan la misma cadena de conexión por defecto:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=prueba_netby;Username=postgres;Password=root"
}
```

- Ajusta la cadena en `ProductService/ProductService.Api/appsettings.json` y `TransactionService/TransactionService.Api/appsettings.json` según tus credenciales/host.
- `TransactionService` necesita la URL pública del `ProductService` para consultar y actualizar stock. Actualiza `ProductService:BaseUrl` en `TransactionService/TransactionService.Api/appsettings.json` si cambias el puerto.

## Cómo ejecutar

### Usando CLI

1. Restaurar dependencias y compilar la solución:
   ```bash
   dotnet restore InventorySystem.sln
   dotnet build InventorySystem.sln
   ```
2. Levantar primero el **ProductService**:
   ```bash
   dotnet run --project ProductService/ProductService.Api
   ```
   Swagger quedará disponible en `https://localhost:57873/swagger` (o `http://localhost:57875` en HTTP).
3. Levantar el **TransactionService** en otra terminal:
   ```bash
   dotnet run --project TransactionService/TransactionService.Api
   ```
   Swagger quedará disponible en `https://localhost:57872/swagger` (o `http://localhost:57874`).

### Usando Visual Studio

1. Abrir `InventorySystem.sln`.
2. Seleccionar `ProductService.Api` como proyecto de inicio y ejecutarlo.
3. Luego ejecutar `TransactionService.Api` (puede configurarse como segundo proyecto de inicio o en otra instancia).

## Endpoints principales

### ProductService
- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `POST /api/products/{id}/increase-stock?quantity=Q`
- `POST /api/products/{id}/decrease-stock?quantity=Q`
- `GET /api/products/search?name=ABC` — búsqueda binaria sobre lista ordenada en memoria para alto volumen de lecturas.

### TransactionService
- `POST /api/transactions` — registra compras/ventas, valida stock llamando a `ProductService` y luego ajusta el stock.
- `GET /api/transactions/history?productId=...&from=...&to=...&type=...` — historial filtrado por producto, fecha y tipo.

## Notas de arquitectura

- Comunicación síncrona entre microservicios vía HTTP (`HttpClient`).
- Separación en capas: Domain (entidades/puertos), Application (casos de uso), Infrastructure (adaptadores, EF Core/Npgsql), Api (endpoints HTTP).
- Índice `ix_products_name` y caché en memoria con lista ordenada para búsquedas rápidas por nombre.

## Resolución de problemas comunes

- **Certificados HTTPS locales**: si usas `dotnet dev-certs`, asegúrate de confiar en el certificado para que Swagger funcione en HTTPS.
- **Errores de conexión a PostgreSQL**: verifica host/puerto/credenciales en `appsettings.json` y que la base de datos `prueba_netby` exista.
- **BaseUrl incorrecto en TransactionService**: si cambias el puerto del ProductService, actualiza `ProductService:BaseUrl` antes de registrar transacciones.
