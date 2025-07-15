# Employee Management System

A web application built with **ASP.NET Core MVC** using **Entity Framework Core**, designed for managing employees and departments within a company.

## Features

### Employees
- Browse the list of employees
- Search employees by name, surname, or other criteria
- Sort employees by various columns (e.g. last name, department)
- Add a new employee
- Edit existing employee details
- Delete an employee

### Departments
- View list of departments
- Add a new department
- Delete a department

## Technologies Used

- **ASP.NET Core MVC** – application architecture based on the Model-View-Controller pattern
- **Razor Views** – server-side rendering engine used for generating dynamic HTML views
- **Entity Framework Core** – object-relational mapping (ORM) for database operations
- **C#** – backend programming language
- **SQL Server – relational database

## Running the Project Locally
1. Clone the repository
2. Apply database migrations: dotnet ef database update
3. Open your browser and go to:
  http://localhost:5000
  or
  https://localhost:5001 if using HTTPS
