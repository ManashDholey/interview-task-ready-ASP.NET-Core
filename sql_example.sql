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
