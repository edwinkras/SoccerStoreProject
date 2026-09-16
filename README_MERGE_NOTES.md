# Soccer Store Monorepo

Backend, frontend, and database now actually wired together, not just
sitting in the same folder.

## What changed in this merge

- **`database/`** — the real schema, seed data, views, functions, and
  queries, tested against an actual local PostgreSQL install (not just
  written and assumed correct). See `database/README.md` for details on
  each file.
- **`infra/podman/initdb/`** — numbered copies of the schema/seed/views/
  functions scripts (`01_schema.sql` through `04_functions.sql`). The
  official Postgres image automatically runs every `.sql` file in this
  folder, in order, the first time the container starts with an empty
  data volume.
- **`apps/api/SoccerStore.Api/Models/Item.cs`** — updated to use
  `CategoryId` (a foreign key) instead of a plain enum, matching the real
  `items` table.
- **`apps/api/SoccerStore.Api/Models/Category.cs`** — new, matches the
  `categories` table.
- **`apps/api/SoccerStore.Api/Data/StoreContext.cs`** — now maps the
  `Category` relationship and uses snake_case naming (via the added
  `EFCore.NamingConventions` package) so C# property names like
  `CategoryId` map to the real `category_id` column automatically.
- **`Program.cs`** — the `/api/items` filter now takes `categoryId`
  instead of a category name. A new `/api/categories` endpoint lists the
  available categories. The `/sell` endpoint now calls the actual
  `sell_item()` database function instead of just editing a number, so a
  real sale record gets created too.

## Important: reset your existing containers before testing

If you already ran `podman-compose up` on this project before, a data
volume called `soccerstore-pgdata` already exists on your machine, empty,
with no tables. Postgres only runs the `initdb` scripts the **first** time
it starts with a completely empty volume, so simply rebuilding will not
pick up the new schema.

Reset it first:

```bash
cd infra/podman
podman-compose down -v
podman-compose up -d --build
```

The `-v` flag deletes the old empty volume so Postgres initializes fresh
and actually runs the schema this time.

## Verifying it worked

```bash
curl http://localhost:8080/db-check
curl http://localhost:8080/api/categories
curl http://localhost:8080/api/items
```

`/api/items` should now return real seeded items instead of a 500 error.

## A note on what I could and couldn't verify

The SQL in `database/` was tested directly against a real PostgreSQL 16
instance, every table, constraint, view, function, and query actually ran
and returned correct results.

The C# changes to `Item.cs`, `Category.cs`, `StoreContext.cs`, and
`Program.cs` were written carefully to match that schema, but could not be
compiled in the environment this was built in, no .NET SDK or NuGet
access there. Run `dotnet build` yourself (or just try the full
`podman-compose up --build`) to confirm it compiles cleanly, and let me
know if anything doesn't, so we can fix it together.
