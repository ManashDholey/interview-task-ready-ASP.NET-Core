SELECT
    CustomerId,
    SUM(Amount) AS TotalAmount
FROM Transactions
WHERE TransactionDate >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
  AND TransactionDate < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
GROUP BY CustomerId;

-- Replace 3 with your desired 'N' value
WITH RankedSalaries AS (
    SELECT 
        Salary,
        DENSE_RANK() OVER (ORDER BY Salary DESC) AS SalaryRank
    FROM Employee
)
SELECT DISTINCT Salary 
FROM RankedSalaries 
WHERE SalaryRank = 3; 
GO
Select *
FROM Employees3
Where DATEDIFF(MONTH, HireDate, GETDATE()) Between 1 and 6

CREATE NONCLUSTERED INDEX IX_Employees3_HireDate 
ON dbo.Employees3 (HireDate);

SELECT * 
FROM Employees3 
WHERE HireDate >= DATEADD(MONTH, -6, CAST(GETDATE() AS DATE))
 AND HireDate <  DATEADD(MONTH, -1, CAST(GETDATE() AS DATE));
GO
--SELECT * FROM Employees2;
WITH EmployeeCTE AS (

SELECT *, ROW_NUMBER() OVER (PARTITION BY ID ORDER BY ID ) AS RowNumber FROM Employees2

)
-- SELECT * FROM EmployeeCTE;

DELETE FROM EmployeeCTE WHERE RowNumber > 1

GO

DECLARE @ID INT 
SET @ID = 10

;WITH EmployeeCTE AS (
SELECT EmployeeId , EmployeeName , ManagerID 
FROM Employees1 WHERE EmployeeId = @ID

UNION ALL 

SELECT e.EmployeeId , e.EmployeeName , e.ManagerID 
FROM Employees1 As e  
INNER JOIN EmployeeCTE AS ec ON e.EmployeeId = ec.ManagerID
)

SELECT E1.EmployeeName, E2.EmployeeName as ManagerName
FROM EmployeeCTE E1 INNER JOIN EmployeeCTE E2 ON E1.ManagerID = E2.EmployeeId
GO

SELECT Max(Salary) AS Salary FROM Employees 
WHERE Salary <(SELECT Max(Salary) FROM Employees)
SELECT Top 1  Salary  FROM ( SELECT DISTINCT TOP 2 Salary FROM Employees ORDER BY Salary DESC) AS Result ORDER BY Salary
WITH Result AS (
SELECT Salary , DENSE_RANK() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result WHERE RankSalary =3

GO
SELECT Max(Salary) AS Salary FROM Employees 
WHERE Salary <(SELECT Max(Salary) FROM Employees)

SELECT Top 1  Salary  FROM ( SELECT DISTINCT TOP 2 Salary FROM Employees ORDER BY Salary DESC) AS Result ORDER BY Salary  

;WITH Result AS (
SELECT Salary , DENSE_RANK() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result WHERE RankSalary =3

;WITH Result1 AS (
SELECT Salary , ROW_NUMBER() over (order by Salary) as RankSalary FROM Employees
)
SELECT * FROM Result1 WHERE RankSalary =3


