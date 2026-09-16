-- Soccer Store database schema
-- Tables: categories, items, suppliers, restocks, sales, sale_items (6 total)

CREATE TABLE categories (
    id          SERIAL PRIMARY KEY,
    name        VARCHAR(20) NOT NULL UNIQUE
        CHECK (name IN ('Cleats','Shirts','Socks','Bags','Jackets','Shorts','Pants','Balls'))
);

CREATE TABLE items (
    id              SERIAL PRIMARY KEY,
    name            VARCHAR(100) NOT NULL,
    category_id     INTEGER NOT NULL REFERENCES categories(id),
    price           NUMERIC(10,2) NOT NULL CHECK (price > 0),
    stock_quantity  INTEGER NOT NULL DEFAULT 0 CHECK (stock_quantity >= 0),
    -- Size only applies to balls (regulation sizes 3, 4, 5). NULL for everything else.
    size            INTEGER CHECK (size IS NULL OR size IN (3, 4, 5)),
    created_at      TIMESTAMP NOT NULL DEFAULT now()
);

CREATE TABLE suppliers (
    id              SERIAL PRIMARY KEY,
    name            VARCHAR(100) NOT NULL UNIQUE,
    contact_email   VARCHAR(150)
);

CREATE TABLE restocks (
    id              SERIAL PRIMARY KEY,
    item_id         INTEGER NOT NULL REFERENCES items(id),
    supplier_id     INTEGER NOT NULL REFERENCES suppliers(id),
    quantity        INTEGER NOT NULL CHECK (quantity > 0),
    order_date      DATE NOT NULL DEFAULT CURRENT_DATE,
    received        BOOLEAN NOT NULL DEFAULT false
);

CREATE TABLE sales (
    id              SERIAL PRIMARY KEY,
    sale_date       TIMESTAMP NOT NULL DEFAULT now()
);

CREATE TABLE sale_items (
    id              SERIAL PRIMARY KEY,
    sale_id         INTEGER NOT NULL REFERENCES sales(id),
    item_id         INTEGER NOT NULL REFERENCES items(id),
    quantity_sold   INTEGER NOT NULL CHECK (quantity_sold > 0),
    unit_price      NUMERIC(10,2) NOT NULL CHECK (unit_price > 0)
);

CREATE INDEX idx_items_category ON items(category_id);
CREATE INDEX idx_sale_items_sale ON sale_items(sale_id);
CREATE INDEX idx_sale_items_item ON sale_items(item_id);
CREATE INDEX idx_restocks_item ON restocks(item_id);
