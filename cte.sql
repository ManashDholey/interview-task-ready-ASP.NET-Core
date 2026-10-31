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