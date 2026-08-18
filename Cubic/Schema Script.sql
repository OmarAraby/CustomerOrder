----Task 2: SQL-Server Database 
use CubicDB;
go

-- Employee Table
create table Employees (
	EmployeeID int primary key identity(1,1),
	FirstName nvarchar(50),
	LastName nvarchar(50),
	DateOfJoining date,
	DepartmentID int foreign Key references Departments(DepartmentID) ,
	isActive bit default 1
);
go

-- department Table
create table Departments (
	DepartmentID int primary key identity(1,1),
	DepartmentName nvarchar(50) not null
);
go
-- sample data for Departments
insert into Departments (DepartmentName) values 
('HR'),
('Finance'),
('IT'),
('Sales');


-- PerformanceReviews Table
create table PerformanceReviews (
	ReviewID int primary key identity(1,1),
	EmployeeID int foreign key references Employees(EmployeeID),
	ReviewDate date,
	Score int check (Score between 1 and 10),
	Comments nvarchar(255)
);
go


------------------------ Views ------------------------
create view vw_EmployeeDetails as
select 
	e.EmployeeID,
	e.FirstName,
	e.LastName,
	e.DateOfJoining,
	e.isActive,
	d.DepartmentName
from Employees e 
inner join Departments d
on e.DepartmentID = d.DepartmentID;
go 

--- test view
select * from vw_EmployeeDetails;
go


--- ---------------------- Indexes ------------------------

--- non-clustered index on DateOfJoining
create nonclustered index IX_Employees_DateOfJoining
on employees (DateOfJoining);
go 


--- non-clustered index on empId, ReviewDate
create nonclustered index IX_PerformanceReviews_EmployeeID_ReviewDate
on PerformanceReviews (EmployeeID, ReviewDate);
go



---------- Functions ------------------------

-- scalar fn to get experience of emp based on DateOfJoining
create function fn_GetExperience(@DateOfJoining date)
returns int
as 
begin
	declare @Experience int;
	set @Experience = datediff(year, @DateOfJoining, getdate());
	return @Experience;
end;
go

--- scalar fn to get experience of emp based on DateOfJoining using EmployeeID as param
Create function fn_GetExperience2(@EmployeeID int)
returns int
begin
	declare @DateOfJoining date;
	declare @Experience int;
	select @DateOfJoining = DateOfJoining
	from Employees 
	where EmployeeID = @EmployeeID;
	set @Experience = datediff(year, @DateOfJoining, getdate());
	return @Experience;
end;
go

---- test functions

select fn_GetExperience('2020-01-01') AS ExperienceYears;
go


---- fn for get to n emp based on avg performance score

create function fn_GetTopPerformers(@TopN int)
returns table
as 
return
(
	select top (@TopN) e.EmployeeID, e.FirstName, e.LastName,d.DepartmentName, avg(pr.Score) as AvgScore
	from Employees e
	inner join PerformanceReviews pr on e.EmployeeID = pr.EmployeeID
	inner join Departments d on e.DepartmentID = d.DepartmentID
	group by e.EmployeeID, e.FirstName, e.LastName, d.DepartmentName
	order by AvgScore desc
);
go


-- test 
SELECT * FROM fn_GetTopPerformers(2);



----------- Stored Procedures ------------------------

--- sp ---->  usp_AddEmployee 
create or alter procedure usp_AddEmployee
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@DateOfJoining date,
	@DepartmentID int,
	@isActive bit = 1

	as
begin
	begin try
		if not exists (select 1 from Departments where DepartmentID = @DepartmentID)
			begin
				raiserror('Invalid DepartmentID. Department does not exist', 16, 1);
				return;
			end

			insert into Employees (FirstName, LastName, DateOfJoining, DepartmentID, isActive)
			values (@FirstName, @LastName, @DateOfJoining, @DepartmentID, @isActive);

			print 'Employee added successfully.';
			RETURN 0;
	end try 
	begin catch
		print 'Error occurred while adding employee: ' + ERROR_MESSAGE();
		RETURN 1;
	end catch

	end;

	-- test
EXEC usp_AddEmployee 'Omar', 'Araby', '2026-01-01', 1;
GO




--- sp2 usp_AddPerformanceReview
create or alter procedure usp_AddPerformanceReview @EmpId int , @RDate Date, @Score int , @Commnets nvarchar(255) = null
as
begin
	begin try
		if not Exists( select 1 from Employees where EmployeeID = @EmpId)
		begin
			 RAISERROR('Employee does not exist', 16, 1);
			 RETURN;
		end
		insert into PerformanceReviews(EmployeeID,ReviewDate,Score,Comments)
		values(@EmpId,@RDate,@Score,@Commnets);

		print 'Performance review added successfully.';
		RETURN 0;
	end try

	begin catch
		select ERROR_MESSAGE() AS [Error Message];
		RETURN 1;
	end catch
end;
go


--- test
EXEC usp_AddPerformanceReview 1, '2026-01-01', 8, 'Great job!';
go



---- sp >> usp_GetDepartmentStatistics
create or alter proc usp_GetDepartmentStatistics
as 
begin
	select d.DepartmentID, d.DepartmentName, 
		count(e.EmployeeID) as TotalEmployees,
		avg(pr.Score) as AveragePerformanceScore
	from Departments d
	left join Employees e on e.DepartmentID = d.DepartmentID
	left join PerformanceReviews pr on e.EmployeeID = pr.EmployeeID
	group by d.DepartmentID, d.DepartmentName
	order by AveragePerformanceScore desc;

end;
go

--test 
EXEC usp_GetDepartmentStatistics;
go



--- sp -->  usp_GetDynamicReport
create or alter procedure usp_GetDynamicReport
	@DepartmentID int = null,
	@MinimumScore int = null,
	@StartDate date = null,
	@EndDate date = null
as
begin 
	select e.EmployeeID, e.FirstName, e.LastName, d.DepartmentName, pr.ReviewDate, pr.Score, pr.Comments
	from Employees e
	inner join Departments d on e.DepartmentID = d.DepartmentID
	inner join PerformanceReviews pr on e.EmployeeID = pr.EmployeeID
	where (@DepartmentID is null or e.DepartmentID = @DepartmentID)
		and (@MinimumScore is null or pr.Score >= @MinimumScore)
		and (@StartDate is null or pr.ReviewDate >= @StartDate)
		and (@EndDate is null or pr.ReviewDate <= @EndDate)
		and e.isActive = 1;
end;
go

EXEC usp_GetDynamicReport;
go

EXEC usp_GetDynamicReport @DepartmentID=1;
go

EXEC dbo.usp_GetDynamicReport @MinimumScore = 8, @StartDate = '2024-01-01';
go



------ trigger----------
--- Create a trigger trg_UpdateEmployeeStatus 
create trigger trg_UpdateEmployeeStatus on PerformanceReviews
after INSERT, UPDATE
as
begin 
	update e
	set isActive = 0
	from Employees e
	inner join (
		select EmployeeID, AVG(Score) as AvgScore
		from PerformanceReviews 
		group by EmployeeID
		having AVG(Score) < 3
	) pr on e.EmployeeID = pr.EmployeeID;
end;
go

