🎟️ TicketManager – Event Ticketing Platform
TicketManager is a lightweight and modern ticketing system built with ASP.NET Core MVC, designed to make event discovery and ticket buying quick and hassle-free.
It offers secure authentication, admin role management, event categorization, and instant ticket purchase – all organized in a clean layered architecture.

🌟 Key Highlights

🔐 Secure Authentication & Identity
(Google, Discord and GitHub logins)

🎫 Event Catalog & Ticket Management
(CRUD operations for events and tickets)

⚡ Instant Purchase Flow
(Simple “Buy → Done” process)

🗂️ Category Management (Admin & Manager roles)

🏗️ Clean Architecture with layered design separating business logic and data access

♻️ Global Constants & Validations for consistent data handling

🧪 Unit Testing Support for core business logic

🏛️ Dedicated Admin Area for managing events and categories
(Accessible to Admin and Manager roles; only Admin can permanently delete events)

🖼️ Preview (Screenshots)
(Place screenshots in screenshots/ folder and update paths below)

🏠 Homepage
![alt text](image.png)

📄 Event Details Page
![alt text](image.png)

🛠️ Admin – Event Management
![alt text](image-1.png)
👤 Test Accounts
Administrator
✉️ Email: admin@TManager.com
🔑 Password: Admin123!

Manager
✉️ Email: manager@TManager.com
🔑 Password: Mananger123!

Standard User
✉️ Email: user@TManager.com
🔑 Password: User123!

🛠 Tech Stack
⚙️ ASP.NET Core 8 MVC – Backend & UI framework

🗄️ Entity Framework Core + SQL Server – ORM & database layer

🔑 ASP.NET Identity – Authentication & roles

🎨 Bootstrap 5 / FontAwesome – Frontend styling

🧪 Unit Testing – NUnit

🌐 External Auth Providers – Google, Discord, GitHub

📂 Solution Structure

📁 TicketManager.Data – DbContext, entity configurations, migrations

📁 TicketManager.Data.Models – Domain entities (Event, Ticket, Order, etc.)

📁 TicketManager.GCommon – Shared constants and validation rules

📁 TicketManager.Services – Core services and interfaces

📁 TicketManager.Web.ViewModels – ViewModels for controllers and views

📁 TicketManager.Tests – Unit tests for business logic

📁 TicketManager.Web – MVC layer (controllers, views, Identity setup)

