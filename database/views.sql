-- View 1: current inventory, joining items with their category name
CREATE VIEW current_inventory AS
SELECT
    i.id,
    i.name,
    c.name AS category,
    i.price,
    i.stock_quantity,
    i.size
FROM items i
JOIN categories c ON c.id = i.category_id;

-- View 2: sales summary, revenue and units sold per item
CREATE VIEW sales_summary AS
SELECT
    i.id AS item_id,
    i.name,
    c.name AS category,
    COALESCE(SUM(si.quantity_sold), 0) AS units_sold,
    COALESCE(SUM(si.quantity_sold * si.unit_price), 0) AS revenue
FROM items i
JOIN categories c ON c.id = i.category_id
LEFT JOIN sale_items si ON si.item_id = i.id
GROUP BY i.id, i.name, c.name;
