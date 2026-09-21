-- Function 1: sell an item. Creates the sale, records the line item,
-- and decreases stock, all in one call. This is the actual "sell" action
-- your API's /sell endpoint should be calling instead of just editing
-- stock_quantity directly, since it also keeps a real sales record.
CREATE OR REPLACE FUNCTION sell_item(p_item_id INTEGER, p_quantity INTEGER)
RETURNS INTEGER AS $$
DECLARE
    v_price NUMERIC(10,2);
    v_stock INTEGER;
    v_sale_id INTEGER;
BEGIN
    SELECT price, stock_quantity INTO v_price, v_stock
    FROM items WHERE id = p_item_id;

    IF v_price IS NULL THEN
        RAISE EXCEPTION 'Item % does not exist', p_item_id;
    END IF;

    IF v_stock < p_quantity THEN
        RAISE EXCEPTION 'Not enough stock for item % (have %, need %)', p_item_id, v_stock, p_quantity;
    END IF;

    INSERT INTO sales DEFAULT VALUES RETURNING id INTO v_sale_id;

    INSERT INTO sale_items (sale_id, item_id, quantity_sold, unit_price)
    VALUES (v_sale_id, p_item_id, p_quantity, v_price);

    UPDATE items SET stock_quantity = stock_quantity - p_quantity WHERE id = p_item_id;

    RETURN v_sale_id;
END;
$$ LANGUAGE plpgsql;

-- Function 2: total revenue within a date range, used for reporting
CREATE OR REPLACE FUNCTION total_revenue(p_start DATE, p_end DATE)
RETURNS NUMERIC AS $$
    SELECT COALESCE(SUM(si.quantity_sold * si.unit_price), 0)
    FROM sale_items si
    JOIN sales s ON s.id = si.sale_id
    WHERE s.sale_date::date BETWEEN p_start AND p_end;
$$ LANGUAGE sql;
