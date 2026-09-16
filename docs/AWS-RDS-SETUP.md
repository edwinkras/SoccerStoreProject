# Pointing SoccerStore.Api at AWS RDS for PostgreSQL

This does not run automatically — RDS is provisioned by hand (console or CLI), then the
API is told about it through an environment variable. Nothing about the app changes.

## 1. Create the RDS instance
1. AWS Console → **RDS** → **Create database**.
2. Engine: **PostgreSQL** (pick the same major version you use locally, e.g. 16.x).
3. Templates: **Free tier** (school/personal project) or **Dev/Test**.
4. Settings:
   - DB instance identifier: `soccerstore-db`
   - Master username: `soccerstore`
   - Master password: generate one and save it somewhere safe (password manager / secrets file, never git).
5. Instance size: `db.t3.micro` (or `db.t4g.micro`) is enough for coursework.
6. Storage: default 20 GB gp3 is fine.
7. Connectivity:
   - VPC: default is fine.
   - Public access: **Yes**, only if you need to reach it from your laptop directly. If the API
     will run on EC2/ECS in the same VPC, choose **No** and keep it private instead.
   - VPC security group: create a new one, e.g. `soccerstore-db-sg`.
8. Initial database name: `soccerstore` (Additional configuration section).
9. Create database — it takes a few minutes.

## 2. Open the firewall (security group)
1. RDS → your instance → note the **Endpoint** and **Port** (5432).
2. EC2 → Security Groups → `soccerstore-db-sg` → **Edit inbound rules**.
3. Add a rule: Type `PostgreSQL`, Port `5432`, Source = your IP (`My IP`) for local testing,
   or the security group of whatever compute runs the API in production. Never use `0.0.0.0/0`.

## 3. Build the connection string
```
Host=<your-endpoint>.rds.amazonaws.com;Port=5432;Database=soccerstore;Username=soccerstore;Password=<master-password>;SSL Mode=Require;Trust Server Certificate=true
```

## 4. Give it to the API without hardcoding it
EF Core reads `ConnectionStrings:StoreDatabase`. ASP.NET Core config maps `__` to `:` in environment
variable names, so set:

```bash
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__StoreDatabase="Host=<endpoint>.rds.amazonaws.com;Port=5432;Database=soccerstore;Username=soccerstore;Password=<master-password>;SSL Mode=Require;Trust Server Certificate=true"

dotnet run --project apps/api/SoccerStore.Api
```

If you run the app in a Podman container instead, pass it the same way:
```bash
podman run -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__StoreDatabase="Host=<endpoint>.rds.amazonaws.com;Port=5432;Database=soccerstore;Username=soccerstore;Password=<master-password>;SSL Mode=Require;Trust Server Certificate=true" \
  -p 8080:8080 soccerstore-api
```

`appsettings.Production.json` intentionally ships with an empty connection string so nobody
accidentally commits the real RDS password — the environment variable always wins over the
JSON file.

## 5. Apply the schema
From a machine that can reach the RDS endpoint (VPN/bastion/public access as configured above):
```bash
cd apps/api/SoccerStore.Api
dotnet tool install --global dotnet-ef   # first time only
dotnet ef database update --connection "Host=<endpoint>.rds.amazonaws.com;Port=5432;Database=soccerstore;Username=soccerstore;Password=<master-password>;SSL Mode=Require;Trust Server Certificate=true"
```

## 6. Verify
```bash
curl http://localhost:8080/db-check
# {"connected":true,"provider":"Npgsql.EntityFrameworkCore.PostgreSQL"}
```

## Notes
- Never commit real RDS credentials. Use environment variables, `dotnet user-secrets` for local
  dev, or AWS Secrets Manager for anything long-lived.
- Rotate the master password periodically and after coursework is submitted if it was ever shared.
