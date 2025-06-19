# 🧾 Payroll Management API

A secure and scalable ASP.NET Core Web API for managing employee attendance, overtime, reimbursements, and payroll processing. This project uses JWT-based authentication, role-based authorization, audit logging, and is structured for maintainability and testability.

---

## 🔧 Features

- ✅ Employee check-in / check-out (attendance)
- ✅ Submit overtime (max 3 hours/day, after work)
- ✅ Submit reimbursement requests
- ✅ Generate individual payslips (based on payroll period)
- ✅ Generate admin summary of all payslips
- ✅ JWT Authentication & Authorization
- ✅ Role-based access control (Employee vs Admin)
- ✅ Audit logging for every critical change
- ✅ Request tracing via `RequestId` and IP logging

---

## 📦 Tech Stack

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- SQLite / InMemory (for tests)
- JWT Authentication
- xUnit + FluentAssertions for testing

---

## 🚀 Getting Started

### 1. Clone the Repo

```bash
git clone https://github.com/arumteguh/DeallsTechnicalTest.git
cd DeallsTechnicalTest
```
### 2. Setup Database

Make sure you have SQL Server or SQLite running, then update your `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "your-db-connection-here"
}
```

Run the initial migration with Package Manage Console:

```bash
dotnet ef database update
```
Alternatively, use SQLite or InMemory for local development/testing.

### 3. Run the API

```bash
dotnet run
```

Navigate to:
📍 https://localhost:{port}/swagger

### 4. Authentication
- Use /api/auth/login to get a JWT token.
- Copy the token and click the 🔐 Authorize button in Swagger.
- Paste the token as: Bearer eyJhbGciOiJIUz...
- Admin users will receive a token with "IsAdmin": true claim.

## 📘 API Usage

| Endpoint                                    | Access   | Description                |
| ------------------------------------------- | -------- | -------------------------- |
| `POST /api/attendance/submit`               | Employee | Check-in/check-out         |
| `POST /api/overtime/submit`                 | Employee | Submit overtime (≤ 3h)     |
| `POST /api/reimbursement/submit`            | Employee | Submit reimbursement       |
| `GET /api/payslip/generate/{payroll}`       | Employee | View payslip for a period  |
| `POST /api/attendanceperiod/create`         | Admin    | Define a payroll period    |
| `POST /api/payroll/run/{payroll}`           | Admin    | Run payroll (locks data)   |
| `GET /api/payslipsummary/summary/{payroll}` | Admin    | View take-home pay summary |

## 🧱 Software Architecture

This project follows Clean Layered Architecture:

| Layer/Folder   | Responsibility                                                                 |
| -------------- | ------------------------------------------------------------------------------ |
| `Controllers`  | Handles incoming HTTP requests and returns responses (API endpoints).          |
| `Services`     | Contains business logic; coordinates between repositories and DTOs.            |
| `Repositories` | Handles direct database access using Entity Framework Core.                    |
| `DTO`          | Defines request and response objects (Data Transfer Objects).                  |
| `Models`       | Contains entity classes that represent database tables.                        |
| `Middleware`   | Custom request pipeline logic (e.g., JWT auth, IP logging, RequestId tracing). |
| `Utils`        | Shared helpers and utility functions used across the project.                  |
| `Data`         | Contains `DbContext` and database configuration/seeding logic.                 |
| `Migrations`   | Auto-generated EF Core migration files to manage schema changes.               |


✅ IHttpContextAccessor used for IP address and user tracing

✅ AuditLog table stores all key actions (create/update)

✅ RequestId is injected into each request for traceability


## 📂 Folder Structure

| Folder         | Purpose                                               |
| -------------- | ----------------------------------------------------- |
| `Controllers`  | API endpoints/controllers                             |
| `Services`     | Business logic                                        |
| `Repositories` | Database operations (using EF Core)                   |
| `Models`       | Database entities                                     |
| `DTO`          | Data Transfer Objects for requests/responses          |
| `Middleware`   | Custom request middleware (e.g., RequestId, JWT auth) |
| `Data`         | `DbContext` and database configuration                |
| `Migrations`   | EF Core database schema migrations                    |
| `Utils`        | Shared helpers/utilities                              |

