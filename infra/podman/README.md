# Running SoccerStore with Podman

## Prerequisites
- Podman installed (`podman --version`)
- `podman-compose` installed (`pip install --user podman-compose`, or `sudo apt install podman-compose`)

## Option A — database only (run the API with `dotnet run` for fast local dev loops)
```bash
cd infra/podman
cp .env.example .env      # adjust credentials if you want
podman-compose up -d postgres
podman ps                 # confirm soccerstore-postgres is healthy
```
Then run the API normally against `appsettings.Development.json` (already points at
`localhost:5432`):
```bash
cd apps/api/SoccerStore.Api
dotnet run
curl http://localhost:5041/db-check
```

## Option B — full stack in containers (db + api)
```bash
cd infra/podman
cp .env.example .env
podman-compose up -d --build
podman ps                 # soccerstore-postgres and soccerstore-api should both be running
curl http://localhost:8080/db-check
```

## Useful commands
```bash
podman-compose logs -f api        # tail API logs
podman-compose down               # stop containers, keep the data volume
podman-compose down -v            # stop and wipe the postgres volume (fresh DB next time)
podman exec -it soccerstore-postgres psql -U soccerstore -d soccerstore
```

## If you don't have podman-compose
Podman 4+ ships `podman compose` (no hyphen) using an external provider, or you can start the
same two containers by hand:
```bash
podman network create soccerstore-net

podman run -d --name soccerstore-postgres --network soccerstore-net \
  -e POSTGRES_USER=soccerstore -e POSTGRES_PASSWORD=soccerstore_dev_pw -e POSTGRES_DB=soccerstore \
  -p 5432:5432 -v soccerstore-pgdata:/var/lib/postgresql/data \
  docker.io/library/postgres:16

podman build -t soccerstore-api -f ../../apps/api/Containerfile ../../apps/api

podman run -d --name soccerstore-api --network soccerstore-net \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__StoreDatabase="Host=soccerstore-postgres;Port=5432;Database=soccerstore;Username=soccerstore;Password=soccerstore_dev_pw" \
  -p 8080:8080 soccerstore-api
```
