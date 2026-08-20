# CustomerOrder API

RESTful Web API for **Customers** and **Orders**, built on .NET Framework 4.8 with Onion Architecture.

Customers and Orders have a **many-to-many** relationship with total participation.

---

## Stack

| | |
|---|---|
| Framework | .NET Framework **4.8** (C# 7.3) |
| Web | ASP.NET **Web API 2** (`System.Web.Http`) + OWIN |
| ORM | **Entity Framework 6.5** — Code First + Migrations |
| Database | SQL Server **LocalDB** |
| DI | **Autofac** (`Autofac.WebApi2`) via `IDependencyResolver` |
| Auth | **ASP.NET Identity 2.2** + **JWT** (HMAC-SHA256) over OWIN |
| Validation | **Data Annotations** (Customers) + **FluentValidation** (Orders) |
| Logging | **Serilog** → rolling file |

---

## Solution layout

```
CustomerOrder.slnx
└── src/
    ├── CustomerOrder.Core            ← domain: entities, exceptions, repository contracts
    ├── CustomerOrder.Application     ← DTOs, services, validators, abstractions
    ├── CustomerOrder.Infrastructure  ← EF6, Identity, JWT signing
    └── CustomerOrder.Api             ← controllers, filters, handlers, composition root
```

Dependencies point **inward only**:

```
Api → Infrastructure → Application → Core
```

`CustomerOrder.Core` has **zero** project references and **zero** NuGet packages.

---

## Running it

**1.** Open `CustomerOrder.slnx` in Visual Studio 2022 or later.

**2.** Restore NuGet packages (Build → Rebuild Solution).

**3.** Create the database. In **Package Manager Console**:

```powershell
Update-Database -ProjectName CustomerOrder.Infrastructure -StartUpProjectName CustomerOrder.Api
```

This creates `CustomerOrderDb` on `(localdb)\MSSQLLocalDB`, applies the `Initial` migration, and seeds the two roles and two accounts.

**4.** Press `F5`. The API starts on `https://localhost:44395`.

**5.** Import `postman/CustomerOrder.postman_collection.json` and run **Auth → Login** first — it stores the token automatically for every other request.

The collection holds **one request per endpoint (11 total)**. To check role-based authorization, change the login body to `user` / `User123` and retry an admin-only request: it must answer **403**, not 401.

### Configuration

Everything lives in `src/CustomerOrder.Api/Web.config`:

```xml
<connectionStrings>
  <add name="CustomerOrderDb" connectionString="Data Source=(localdb)\MSSQLLocalDB;..." />
</connectionStrings>

<appSettings>
  <add key="Jwt:Issuer" value="CustomerOrder.Api" />
  <add key="Jwt:Audience" value="CustomerOrder.Client" />
  <add key="Jwt:Key" value="..." />
  <add key="Jwt:ExpiryMinutes" value="60" />
</appSettings>
```

> `Jwt:Key` is a **development-only** value committed for convenience. In anything real it belongs in an environment variable or a secret store. It must be at least 32 characters — HMAC-SHA256 requires a 256-bit key.

---

## Seeded accounts

| Username | Password | Role |
|---|---|---|
| `admin` | `Admin123` | `admin` |
| `user` | `User123` | `user` |

Created by `IdentitySeeder`, which runs from the migrations `Seed` method on every `Update-Database`. Passwords are hashed with ASP.NET Identity's PBKDF2.

---

## Endpoints

All endpoints require a valid JWT except the login. Send it as `Authorization: Bearer <token>`.

| Method | Endpoint | Roles |
|---|---|---|
| POST | `/api/auth/login` | anonymous |
| GET | `/api/customers` | `admin` |
| GET | `/api/customers/{id}` | `user`, `admin` |
| POST | `/api/customers` | `admin` |
| PUT | `/api/customers/{id}` | `admin` |
| DELETE | `/api/customers/{id}` | `admin` |
| GET | `/api/orders` | `user`, `admin` |
| GET | `/api/orders/{id}` | `user`, `admin` |
| POST | `/api/orders` | `user`, `admin` |
| PUT | `/api/orders/{id}` | `user`, `admin` |
| DELETE | `/api/orders/{id}` | `admin` |


### Response shape

Every response — success or failure — uses the same envelope:

```json
{
  "success": true,
  "message": "Customers retrieved successfully",
  "data": { },
  "errors": []
}
```

### Status codes

| Code | When |
|---|---|
| 200 / 201 | Success |
| 400 | Validation failed — `errors` lists the offending fields |
| 401 | No token, invalid token, or wrong credentials |
| 403 | Valid token, but the role is not allowed |
| 404 | Resource not found |
| 409 | Duplicate email / order number, or a total-participation rule |
| 500 | Unexpected — generic message to the client, full detail in the log |
---

## Project layout on disk

```
CustomerOrder/
├── CustomerOrder.slnx
├── README.md
├── postman/
│   └── CustomerOrder.postman_collection.json
├── packages/                      NuGet (packages.config style, not committed)
└── src/
    ├── CustomerOrder.Core/
    │   ├── Entities/  Enums/  Exceptions/  Interfaces/
    ├── CustomerOrder.Application/
    │   ├── Common/  Dtos/  Interfaces/  Services/  Validators/
    ├── CustomerOrder.Infrastructure/
    │   ├── Identity/  Persistence/  Repositories/
    └── CustomerOrder.Api/
        ├── App_Start/  Controllers/  ErrorHandling/  Filters/  Handlers/
        ├── Global.asax  Startup.cs  Web.config
        └── logs/                   Serilog output (not committed)
```
