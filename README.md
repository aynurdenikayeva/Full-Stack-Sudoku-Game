# 🎮 Full-Stack Sudoku Game

A modern and interactive Sudoku web application built with **ASP.NET Core MVC**. The application provides an engaging Sudoku experience with multiple difficulty levels, user authentication, profile management, and an administrative dashboard.

## ✨ Features

### 🧩 Sudoku Gameplay
- Interactive 9×9 Sudoku board
- Automatic Sudoku puzzle generation
- Six difficulty levels:
  - Easy
  - Medium
  - Hard
  - Expert
  - Master
  - Extreme
- Interactive number pad
- Timer to track game duration
- Mistake tracking
- Hint functionality
- Clear and undo actions
- Automatic game completion detection

### 👤 User Management
- User registration and login
- Authentication and authorization with ASP.NET Core Identity
- User profile management
- Profile editing
- User account management

### 🛠️ Admin Dashboard
- Administrative dashboard
- User management
- Profile management
- Application settings
- Registration control
- Contact message management

### 🌐 Additional Pages
- Home page
- About page
- Blog
- FAQ
- Contact page
- Leaderboard

## 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| C# | Backend development |
| ASP.NET Core MVC | Web application framework |
| Entity Framework Core | Database management |
| ASP.NET Core Identity | Authentication and authorization |
| SQL Server | Relational database |
| HTML5 | Page structure |
| CSS3 | Styling and responsive design |
| JavaScript | Client-side interactions |
| Bootstrap | UI components |

## 📁 Project Structure

```text
SudokuExamProject/
│
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       └── Views/
│
├── Controllers/
├── DataAccessLayer/
├── Models/
├── ViewModels/
├── Helpers/
├── Services/
├── Views/
│   ├── Home/
│   ├── Account/
│   ├── Profile/
│   ├── Blog/
│   ├── FAQ/
│   ├── Contact/
│   └── Shared/
│
├── wwwroot/
│   ├── admin/
│   ├── css/
│   ├── js/
│   ├── images/
│   └── lib/
│
├── appsettings.json
└── Program.cs
```

## ⚙️ Installation and Setup

### Prerequisites

Make sure you have installed:

- .NET SDK
- Visual Studio
- SQL Server
- Postre Sql
- SQL Server Management Studio (SSMS)

### 1. Clone the repository

```bash
git clone https://github.com/aynurdenikayeva/Full-Stack-Sudoku-Game.git
```

### 2. Navigate to the project directory

```bash
cd Full-Stack-Sudoku-Game
```

### 3. Configure the database

Update the connection string in your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=SudokuDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}//MSSql
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Port=5432;Database=sudokuGame_db;Username=postgres;Password=;"
  }
}//PostreSql
```

Replace `YOUR_SERVER` with your SQL Server instance name.

### 4. Apply database migrations

If migrations are configured in the project, run:

```bash
add-migration init
add-migration CreateTable
update-database
```

### 5. Run the application

```bash
dotnet restore
dotnet run
```

Open the local URL displayed in the terminal to access the application.

## 📸 Screenshots

Add screenshots of your application here to showcase its interface and functionality.

Suggested screenshots:

- Home Page
- Sudoku Game Board
- Login and Registration
- User Profile
- Admin Dashboard
- Leaderboard

## 🎯 Project Goals

This project was developed to strengthen practical skills in full-stack web development, including:

- Building MVC-based web applications
- Implementing authentication and authorization
- Working with Entity Framework Core
- Managing relational databases
- Developing interactive user interfaces
- Implementing CRUD operations
- Creating administrative functionality

## 👩‍💻 Author

**Aynur Denikayeva**

- GitHub: [@aynurdenikayeva](https://github.com/aynurdenikayeva)

---

## 🚀 Live Demo

The application is built with **ASP.NET Core 9.0** and deployed on MonsterASP.NET.

🌐 **Live Website:** [AXIOMIX – Sudoku Game](https://axiomix-sudoku.runasp.net/)

⭐ If you find this project interesting, feel free to explore the repository!
