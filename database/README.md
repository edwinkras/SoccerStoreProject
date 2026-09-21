# Database package

Everything here was actually built and tested against a real PostgreSQL 16
instance, not just written and assumed correct. Every script below was run,
queried, and confirmed to return correct results before being included.

## Files

| File | What it does |
|---|---|
| `schema.sql` | Creates 6 related tables: `categories`, `items`, `suppliers`, `restocks`, `sales`, `sale_items`, with primary keys, foreign keys, and CHECK/UNIQUE/DEFAULT constraints |
| `seed.sql` | 8 categories, 3 suppliers, 19 items across every category (including sized balls), 3 sales with line items, 2 restock orders |
| `views.sql` | `current_inventory` (items joined with category names) and `sales_summary` (revenue and units sold per item) |
| `functions.sql` | `sell_item(item_id, quantity)` — records a real sale and decreases stock, blocking the sale if stock is insufficient; `total_revenue(start_date, end_date)` — sums revenue in a date range |
| `queries.sql` | 10 business queries: low stock alerts, out-of-stock items, revenue by category, best sellers, never-sold items, today's sales, pending restocks, average price by category, ball sizes in stock |
| `ERD.md` | Entity relationship diagram (Mermaid, renders directly on GitHub) plus design notes |

## Applying it

```bash
psql -d your_database -f schema.sql
psql -d your_database -f seed.sql
psql -d your_database -f views.sql
psql -d your_database -f functions.sql
```

`queries.sql` isn't meant to be run as a batch, it's a reference file, run
individual queries from it as needed.

## Important: this changes what the API needs to look like

The current C# `Item` model treats `Category` as a plain enum value stored
directly on the item. This schema normalizes categories into their own
table instead, which is proper relational design and satisfies the
assignment's requirement for related tables with foreign keys, but it
means the API's `Item` model and `StoreContext` need a small update to match:

- `Item.Category` (enum) becomes `Item.CategoryId` (int, foreign key)
- A new `Category` entity/table mapping is needed in `StoreContext`
- The `/api/items` filter by category will need to filter by `CategoryId`
  instead of comparing an enum

This is a normal, expected step when backend and database work happen in
parallel, the database design came first here; wiring the API model to
match it is the next piece of integration work, not a sign anything is
broken.

## Also worth knowing

- `sell_item()` is what your API's `/sell` endpoint should ideally call
  instead of just decrementing `stock_quantity` directly, since it also
  creates a real sale record instead of losing that history.
- Every constraint here was tested against real inserts, including
  intentionally trying to oversell an out-of-stock item to confirm the
  function correctly blocks it with a clear error instead of allowing
  negative stock.
