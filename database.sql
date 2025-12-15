-- Script para PostgreSQL - base de datos: pruebanetby
-- Ejecutar dentro de la BD pruebanetby

CREATE TABLE IF NOT EXISTS products (
    id uuid NOT NULL PRIMARY KEY,
    name varchar(200) NOT NULL,
    description varchar(500),
    category varchar(100),
    image_url varchar(500),
    price numeric(18,2) NOT NULL,
    stock integer NOT NULL
);

CREATE INDEX IF NOT EXISTS ix_products_name ON products(name);

CREATE TABLE IF NOT EXISTS stock_transactions (
    id uuid NOT NULL PRIMARY KEY,
    product_id uuid NOT NULL,
    date timestamptz NOT NULL,
    type smallint NOT NULL, -- 1 = Purchase, 2 = Sale
    quantity integer NOT NULL,
    unit_price numeric(18,2) NOT NULL,
    detail varchar(500),
    CONSTRAINT fk_stock_transactions_products
        FOREIGN KEY (product_id) REFERENCES products(id)
);

CREATE INDEX IF NOT EXISTS ix_stock_transactions_product_date
    ON stock_transactions(product_id, date);
