# Database Object Documentation

| Object Name | Type | Purpose |
|-------------|------|---------|
| `dbo.Departments` | Table | Stores department information (e.g., HR, IT) to be referenced by employees. |
| `dbo.Employees` | Table | Stores employee details, status, and associated department. |
| `dbo.PerformanceReviews` | Table | Records individual performance review scores and comments for employees. |
| `dbo.vw_EmployeeDetails` | View | Provides a consolidated output of an employee's details along with their department name. |
| `dbo.fn_GetExperience` | Scalar Function | Calculates total years of experience manually based on a provided date. |
| `dbo.fn_GetExperience2` | Scalar Function | Calculates total years of experience by looking up a specific EmployeeID. |
| `dbo.fn_GetTopPerformers` | Table Function | Returns the top N highest performing employees based on their average score. |
| `dbo.usp_AddEmployee` | Stored Procedure | Safely adds a new employee with a safety check ensuring the department is valid. |
| `dbo.usp_AddPerformanceReview` | Stored Procedure | Safely logs a performance review with a safety check ensuring the employee exists. |
| `dbo.usp_GetDepartmentStatistics` | Stored Procedure | Aggregates and returns total employee count and average score grouped by department. |
| `dbo.usp_GetDynamicReport` | Stored Procedure | Generates customized performance reports filtered dynamically by parameters like dates and scores. |
| `trg_UpdateEmployeeStatus` | Trigger | Automatically sets an employee's isActive flag to 0 if their average performance score drops below 3. |