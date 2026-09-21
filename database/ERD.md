# Entity Relationship Diagram

```mermaid
erDiagram
    CATEGORIES ||--o{ ITEMS : contains
    ITEMS ||--o{ SALE_ITEMS : "sold as"
    SALES ||--o{ SALE_ITEMS : includes
    ITEMS ||--o{ RESTOCKS : "restocked via"
    SUPPLIERS ||--o{ RESTOCKS : fulfills

    CATEGORIES {
        int id PK
        varchar name
    }
    ITEMS {
        int id PK
        varchar name
        int category_id FK
        numeric price
        int stock_quantity
        int size
        timestamp created_at
    }
    SUPPLIERS {
        int id PK
        varchar name
        varchar contact_email
    }
    RESTOCKS {
        int id PK
        int item_id FK
        int supplier_id FK
        int quantity
        date order_date
        boolean received
    }
    SALES {
        int id PK
        timestamp sale_date
    }
    SALE_ITEMS {
        int id PK
        int sale_id FK
        int item_id FK
        int quantity_sold
        numeric unit_price
    }
```

## Design notes

- **categories** is a lookup table matching the required item categories
  (Cleats, Shirts, Socks, Bags, Jackets, Shorts, Pants, Balls), enforced with
  a CHECK constraint so nothing else can be inserted.
- **items** is the main inventory table. `size` is nullable and only used
  for balls (regulation sizes 3, 4, 5), enforced with a CHECK constraint.
- **sales** and **sale_items** split a sale into a header (when it happened)
  and its line items (what was actually sold), which is standard practice
  and lets one sale include multiple items.
- **suppliers** and **restocks** track where inventory comes from and
  whether an order has actually arrived, giving the store a real
  restocking workflow instead of stock just changing with no history.
