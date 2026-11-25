-- A Common Table Expression (CTE) is a temporary, named result set that you define within the WITH clause of an 
-- SQL statement. It acts like a temporary view or inline table that exists only for the duration of that single query.
-- A Recursive CTE is a special type of CTE that references itself. It repeatedly runs and builds upon its own output until 
-- it reaches a stopping condition.
-- Example of a simple CTE:
WITH EmployeeCTE AS
(
    SELECT EmployeeID, FirstName, LastName, ManagerID
    FROM Employees
    WHERE ManagerID IS NULL
)
SELECT *
FROM EmployeeCTE;
-- Example of a recursive CTE to get all employees under a specific manager:
WITH EmployeeHierarchy AS
(
    SELECT EmployeeID, FirstName, LastName, ManagerID
    FROM Employees
    WHERE ManagerID = @ManagerID -- Replace with the specific manager's ID

    UNION ALL

    SELECT e.EmployeeID, e.FirstName, e.LastName, e.ManagerID
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ManagerID = eh.EmployeeID
)
SELECT *
FROM EmployeeHierarchy;
Go
    CREATE TABLE [User] (
    Id INT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE TimeCard (
    Id INT,
    Hours DECIMAL(5,2),
    Date DATE
);
GO
    INSERT INTO [User] (Id, Name)
VALUES
(1, 'Manash'),
(2, 'Rahul'),
(3, 'Amit'),
(4, 'Priya'),
(5, 'Sourav');
GO
INSERT INTO TimeCard (Id, Hours, Date)
VALUES
-- Manash (November 2026)
(1, 8, '2026-11-01'),
(1, 7, '2026-11-02'),
(1, 8, '2026-11-03'),
(1, 6, '2026-11-04'),

-- Rahul (November 2026)
(2, 8, '2026-11-01'),
(2, 8, '2026-11-02'),
(2, 5, '2026-11-03'),

-- Amit (November 2026)
(3, 4, '2026-11-01'),
(3, 6, '2026-11-02'),

-- Priya (Previous month - October 2026)
(4, 8, '2026-10-01'),
(4, 8, '2026-10-02'),
(4, 7, '2026-10-03'),

-- Manash (Previous year - November 2025)
(1, 8, '2025-11-01'),

-- Sourav (No time-card records);    
SELECT
    u.Id,
    u.Name,
    COALESCE(SUM(t.Hours), 0) AS TotalHours
FROM [User] u
LEFT JOIN TimeCard t
    ON u.Id = t.Id
    AND t.Date >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
    AND t.Date < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
GROUP BY
    u.Id,
    u.Name
ORDER BY
    u.Name;
