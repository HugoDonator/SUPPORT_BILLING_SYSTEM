CREATE DATABASE SupportBillingDB;
GO

USE SupportBillingDB;
GO

CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Services (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Invoices (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClientId INT NOT NULL,
    InvoiceDate DATETIME NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClientId) REFERENCES Clients(Id) ON DELETE CASCADE
);
CREATE TABLE InvoiceDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvoiceId INT NOT NULL,
    ServiceId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE,
    FOREIGN KEY (ServiceId) REFERENCES Services(Id) ON DELETE CASCADE
);

CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InvoiceId INT NOT NULL,
    AmountPaid DECIMAL(18, 2) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE
);

INSERT INTO Clients (Name, Email, Phone) VALUES
('John Doe', 'john.doe@example.com', '555-1234'),
('Jane Smith', 'jane.smith@example.com', '555-5678');

INSERT INTO Services (Name, Price) VALUES
('PC Repair', 50.00),
('Network Setup', 100.00),
('Data Recovery', 200.00);

INSERT INTO Invoices (ClientId, InvoiceDate, TotalAmount) VALUES
(2, GETDATE(), 150.00);

INSERT INTO InvoiceDetails (InvoiceId, ServiceId, Quantity, Price) VALUES
(1, 1, 1, 50.00),
(1, 2, 1, 100.00);

INSERT INTO Payments (InvoiceId, AmountPaid, PaymentDate) VALUES
(1, 150.00, GETDATE());


ALTER TABLE Invoices
ADD Status NVARCHAR(50) NOT NULL DEFAULT 'Pendiente';


select * from Invoices
select * from InvoiceDetails

ALTER TABLE Invoices
ADD Tax DECIMAL(5, 2) NOT NULL DEFAULT 0.00;


ALTER TABLE Invoices
ADD Subtotal DECIMAL(18, 2) NOT NULL DEFAULT 0.00;
ALTER TABLE InvoiceDetails
ADD Total DECIMAL(18, 2) AS (Quantity * Price) PERSISTED;

ALTER TABLE InvoiceDetails DROP COLUMN Total;

SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'InvoiceDetails';

ALTER TABLE InvoiceDetails
ADD Total AS (Quantity * Price) PERSISTED;