-- 1. All items with current stock, sorted by category
SELECT * FROM current_inventory ORDER BY category, name;

-- 2. Items that are low in stock (below 5 units), a real restocking alert
SELECT name, category, stock_quantity
FROM current_inventory
WHERE stock_quantity < 5
ORDER BY stock_quantity ASC;

-- 3. Items that are completely out of stock
SELECT name, category FROM current_inventory WHERE stock_quantity = 0;

-- 4. Total revenue by category, using the sales_summary view
SELECT category, SUM(revenue) AS category_revenue
FROM sales_summary
GROUP BY category
ORDER BY category_revenue DESC;

-- 5. Best selling items by quantity, top 5
SELECT name, category, units_sold
FROM sales_summary
ORDER BY units_sold DESC
LIMIT 5;

-- 6. Items that have never been sold
SELECT name, category FROM sales_summary WHERE units_sold = 0;

-- 7. All sales made today
SELECT s.id AS sale_id, i.name, si.quantity_sold, si.unit_price
FROM sales s
JOIN sale_items si ON si.sale_id = s.id
JOIN items i ON i.id = si.item_id
WHERE s.sale_date::date = CURRENT_DATE;

-- 8. Pending restock orders with their supplier
SELECT i.name AS item, s.name AS supplier, r.quantity, r.order_date
FROM restocks r
JOIN items i ON i.id = r.item_id
JOIN suppliers s ON s.id = r.supplier_id
WHERE r.received = false;

-- 9. Average price per category
SELECT category, ROUND(AVG(price), 2) AS avg_price
FROM current_inventory
GROUP BY category
ORDER BY avg_price DESC;

-- 10. All ball sizes currently in stock
SELECT name, size, stock_quantity
FROM current_inventory
WHERE category = 'Balls'
ORDER BY size;
