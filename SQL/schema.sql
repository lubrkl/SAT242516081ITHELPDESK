CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE Priorities (
    PriorityId INT PRIMARY KEY IDENTITY(1,1),
    PriorityName NVARCHAR(50) NOT NULL,
    ColorCode NVARCHAR(20)
)
GO

CREATE TABLE Statuses (
    StatusId INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) NOT NULL,
    ColorCode NVARCHAR(20)
)
GO

CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentId),
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE Tickets (
    TicketId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    RequesterId INT NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeId),
    AssignedToId INT FOREIGN KEY REFERENCES Employees(EmployeeId),
    PriorityId INT NOT NULL FOREIGN KEY REFERENCES Priorities(PriorityId),
    StatusId INT NOT NULL FOREIGN KEY REFERENCES Statuses(StatusId),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME
)
GO

CREATE TABLE Logs_Table (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    TableName NVARCHAR(100),
    Operation NVARCHAR(20),
    OldData NVARCHAR(MAX),
    NewData NVARCHAR(MAX),
    LogDate DATETIME DEFAULT GETDATE()
)
GO

CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId)
CREATE INDEX IX_Tickets_RequesterId ON Tickets(RequesterId)
CREATE INDEX IX_Tickets_AssignedToId ON Tickets(AssignedToId)
CREATE INDEX IX_Tickets_PriorityId ON Tickets(PriorityId)
CREATE INDEX IX_Tickets_StatusId ON Tickets(StatusId)
CREATE INDEX IX_Tickets_CreatedDate ON Tickets(CreatedDate)
GO

CREATE VIEW vw_Tickets AS
SELECT t.TicketId, t.Title, t.Description, t.RequesterId, t.AssignedToId,
       t.PriorityId, t.StatusId, t.CreatedDate, t.UpdatedDate,
       r.FirstName + ' ' + r.LastName AS RequesterName,
       a.FirstName + ' ' + a.LastName AS AssignedToName,
       p.PriorityName, p.ColorCode AS PriorityColor,
       s.StatusName, s.ColorCode AS StatusColor
FROM Tickets t
INNER JOIN Employees r ON t.RequesterId = r.EmployeeId
LEFT JOIN Employees a ON t.AssignedToId = a.EmployeeId
INNER JOIN Priorities p ON t.PriorityId = p.PriorityId
INNER JOIN Statuses s ON t.StatusId = s.StatusId
GO

CREATE VIEW vw_Employees AS
SELECT e.EmployeeId, e.FirstName, e.LastName, 
       e.FirstName + ' ' + e.LastName AS FullName,
       e.Email, e.Phone, e.DepartmentId, e.CreatedDate,
       d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.DepartmentId
GO

CREATE TYPE TicketType AS TABLE (
    Title NVARCHAR(200),
    Description NVARCHAR(MAX),
    RequesterId INT,
    AssignedToId INT,
    PriorityId INT,
    StatusId INT
)
GO

CREATE TYPE EmployeeType AS TABLE (
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    DepartmentId INT
)
GO