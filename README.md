# EmployeeManag_APIController

This is an `API` built using `.NET 6` that retrieves `SQL` data using `ADO` and is tested using `Swagger`.

To create a new database, execute the following SQL command:
(

CREATE DATABASE Management

USE Management

CREATE TABLE Employees (

    ID INT PRIMARY KEY IDENTITY,
    
    NAMES NVARCHAR(255) NULL,
    
    DateOfBirth DATE
    
)

INSERT INTO Employees(NAMES, DateOfBirth) VALUES (N'Thúy Kiều', '1997-10-9')

INSERT INTO Employees(NAMES, DateOfBirth) VALUES (N'Từ Hải', '1995-12-5')

INSERT INTO Employees(NAMES, DateOfBirth) VALUES (N'Sở Khanh', '1998-10-9')

INSERT INTO Employees(NAMES, DateOfBirth) VALUES (N'Tú Bà', '1988-5-9')

)

# Explanation:

The API creates an `Employee` object in `/Models/Employees.cs`.
SQL data is retrieved in `/Models/EmpDbContext.cs`.
The `Controllers` directory contains an API Controller that includes the following actions: `Get`, `Get(id)`, `Post`, `Put`, and `Delete`, which interact with the database.

You can test the API using Swagger by following these steps:
1. Press `F5` to run the API.
2. In your browser, access `localhost:[port]/swagger`.
