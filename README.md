# Factify - Backend API

This repository contains the backend server for the **Factify Social Media Learning Platform**. It is built with ASP.NET Core and is responsible for managing the database, handling business logic, user authentication, and serving data to the frontend client via a RESTful API and SignalR hubs.

### ✨ Core Responsibilities
-   **User Authentication & Authorization** using JWT.
-   **RESTful API** for managing posts, users, friends, and interactions.
-   **Personalized Feed Generation Algorithm** to deliver relevant content.
-   **Real-time Communication** for chat and notifications using SignalR.
-   **Database Management** with Entity Framework Core.
-   **Content Moderation System** for handling user reports.

---

## 🛠️ Tech Stack

-   **.NET 8**
-   **ASP.NET Core**
-   **Entity Framework Core**
-   **SignalR**
-   **Microsoft SQL Server**
-   **JWT (JSON Web Tokens)** for Authentication

---

## ⚙️ Installation & Setup

To run the backend server locally, follow these steps.

### Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
-   [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (e.g., Express Edition)

### Setup Instructions

```bash
# 1. Clone this repository
git clone https://github.com/m1toi/Factify-BE.git
cd Factify-BE/

# 2. Configure your application settings in `appsettings.json`
# You must update the database connection string and the JWT secret key.
