-- Seed data for the Soccer Store

INSERT INTO categories (name) VALUES
    ('Cleats'), ('Shirts'), ('Socks'), ('Bags'), ('Jackets'), ('Shorts'), ('Pants'), ('Balls');

INSERT INTO suppliers (name, contact_email) VALUES
    ('Nike Wholesale', 'orders@nikewholesale.example'),
    ('Adidas Distribution', 'sales@adidasdist.example'),
    ('Local Kit Supplier', 'hello@localkit.example');

-- Items: at least a few per category, including balls with sizes
INSERT INTO items (name, category_id, price, stock_quantity, size) VALUES
    ('Nike Mercurial Vapor', (SELECT id FROM categories WHERE name = 'Cleats'), 189.99, 12, NULL),
    ('Adidas Predator', (SELECT id FROM categories WHERE name = 'Cleats'), 174.99, 8, NULL),
    ('Puma Future Z', (SELECT id FROM categories WHERE name = 'Cleats'), 159.99, 0, NULL),

    ('Home Match Jersey', (SELECT id FROM categories WHERE name = 'Shirts'), 89.99, 25, NULL),
    ('Away Match Jersey', (SELECT id FROM categories WHERE name = 'Shirts'), 89.99, 15, NULL),
    ('Training Jersey', (SELECT id FROM categories WHERE name = 'Shirts'), 44.99, 30, NULL),

    ('Grip Socks', (SELECT id FROM categories WHERE name = 'Socks'), 14.99, 50, NULL),
    ('Classic Team Socks', (SELECT id FROM categories WHERE name = 'Socks'), 9.99, 40, NULL),

    ('Duffel Bag', (SELECT id FROM categories WHERE name = 'Bags'), 39.99, 10, NULL),
    ('Boot Bag', (SELECT id FROM categories WHERE name = 'Bags'), 24.99, 3, NULL),

    ('Training Rain Jacket', (SELECT id FROM categories WHERE name = 'Jackets'), 64.99, 6, NULL),
    ('Anthem Jacket', (SELECT id FROM categories WHERE name = 'Jackets'), 74.99, 5, NULL),

    ('Match Shorts', (SELECT id FROM categories WHERE name = 'Shorts'), 34.99, 20, NULL),
    ('Training Shorts', (SELECT id FROM categories WHERE name = 'Shorts'), 24.99, 18, NULL),

    ('Training Pants', (SELECT id FROM categories WHERE name = 'Pants'), 49.99, 14, NULL),
    ('Track Pants', (SELECT id FROM categories WHERE name = 'Pants'), 44.99, 9, NULL),

    ('Match Ball', (SELECT id FROM categories WHERE name = 'Balls'), 129.99, 10, 5),
    ('Youth Ball', (SELECT id FROM categories WHERE name = 'Balls'), 69.99, 15, 4),
    ('Mini Ball', (SELECT id FROM categories WHERE name = 'Balls'), 39.99, 20, 3);

-- Some sales history so views, functions, and queries have real data to work with
INSERT INTO sales (sale_date) VALUES
    (now() - interval '3 days'),
    (now() - interval '1 day'),
    (now());

INSERT INTO sale_items (sale_id, item_id, quantity_sold, unit_price) VALUES
    (1, (SELECT id FROM items WHERE name = 'Nike Mercurial Vapor'), 2, 189.99),
    (1, (SELECT id FROM items WHERE name = 'Match Ball'), 1, 129.99),
    (2, (SELECT id FROM items WHERE name = 'Home Match Jersey'), 3, 89.99),
    (2, (SELECT id FROM items WHERE name = 'Grip Socks'), 5, 14.99),
    (3, (SELECT id FROM items WHERE name = 'Match Shorts'), 2, 34.99),
    (3, (SELECT id FROM items WHERE name = 'Youth Ball'), 1, 69.99);

-- A couple of restock orders, one already received, one still pending
INSERT INTO restocks (item_id, supplier_id, quantity, order_date, received) VALUES
    ((SELECT id FROM items WHERE name = 'Puma Future Z'),
     (SELECT id FROM suppliers WHERE name = 'Local Kit Supplier'), 15, CURRENT_DATE - 2, false),
    ((SELECT id FROM items WHERE name = 'Boot Bag'),
     (SELECT id FROM suppliers WHERE name = 'Nike Wholesale'), 20, CURRENT_DATE - 5, true);
