-- Created by GitHub Copilot in SSMS - review carefully before executing

USE CubicDB;
GO

-- Insert Departments
INSERT INTO dbo.Departments (DepartmentName) 
VALUES ('HR'), ('Finance'), ('IT'), ('Sales');
GO

-- Insert Employees
INSERT INTO dbo.Employees (FirstName, LastName, DateOfJoining, DepartmentID, isActive) 
VALUES 
('Alice', 'Smith', '2019-03-15', 3, 1),
('Bob', 'Johnson', '2021-07-01', 1, 1),
('Charlie', 'Brown', '2020-11-20', 4, 1),
('Diana', 'Prince', '2018-05-10', 2, 1),
('Evan', 'Wright', '2022-01-10', 3, 1);
GO

-- Insert Performance Reviews
INSERT INTO dbo.PerformanceReviews (EmployeeID, ReviewDate, Score, Comments) 
VALUES 
(1, '2023-01-15', 9, 'Excellent problem solving.'),
(1, '2024-01-15', 8, 'Consistent performer.'),
(2, '2023-02-10', 7, 'Good work overall.'),
(3, '2023-03-05', 4, 'Needs improvement in quarterly targets.'),
(4, '2024-01-20', 10, 'Outstanding leadership skills.'),
(5, '2024-02-01', 2, 'Severely lacking motivation; missing deadlines.'); 

GO