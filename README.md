# Soccer Store Monorepo

A real Nx workspace this time, generated with create-nx-workspace and
nx g @nx/angular:app, not just files copied into a folder named right.
The Angular build was actually run and verified to succeed before this
was packaged.

## Structure

```
apps/
  api/        .NET Minimal API (backend)
  web/        Angular frontend, generated through Nx
  web-e2e/    Playwright end to end test scaffold (from the Nx generator)
infra/
  podman/     Container orchestration for the API and PostgreSQL
docs/
  AWS-RDS-SETUP.md   Guide for the AWS hosted PostgreSQL setup
nx.json, tsconfig.base.json, package.json   Nx workspace root files
```

## What's fixed in this version

- The Containerfile no longer splits dotnet restore and
  dotnet publish --no-restore into separate steps. That split caused a
  NuGet analyzer package to go missing at publish time. It's now a single
  dotnet publish step that resolves everything itself.
- apps/web is a properly generated Nx Angular application, not a plain
  Angular CLI app placed in a folder. It was generated with
  nx g @nx/angular:app apps/web.
- The four page components (home, cleats, jerseys, balls) were
  copied in from the original frontend and wired into app.routes.ts,
  including balls, which existed as a component but wasn't routed yet.
- Bootstrap is installed and imported globally in src/styles.scss.
- The placeholder Nx welcome component was removed and the app shell
  (app.ts / app.html) now just renders <router-outlet>.

## Getting started

```
npm install
npx nx serve web       # runs the Angular dev server
npx nx build web        # production build, already verified to succeed
```

## Running the backend

```
cd infra/podman
cp .env.example .env
podman-compose up -d --build
curl http://localhost:8080/db-check
```

Swagger is available at http://localhost:8080/swagger once running.

## Still needed

The database side (schema, ERD, seed data, views, functions, SQL
queries, migrations) is not yet included anywhere in this repo. Only
infrastructure and the AWS setup guide have been delivered so far.

## A note on folder naming

Extract this somewhere with a plain path, avoid # or other special
characters anywhere in the folder chain (e.g. a parent folder named
"C# Angular"). Several JavaScript and Nx tools mishandle # in file
paths, which caused broken builds earlier in this project.
