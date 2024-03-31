# Paylocity Benefits Calculator
This is WebAPI written in .NET 6 to:
- Be able to retrieve employee information
- Be able to retrive dependent information of employees
- Be able to calculate an employee's paycheck

## How to Run the API
This is a standard WebAPI applciation. Just run or debug the API projct in Visual Studio or through the dotnet cli. No authentication was required so you can hit the endpoints via the swagger interface or your favorite API tool (e.g. - Postman, Insomnia, etc.)

## Requirements
- Able to view employees and their dependents
- An employee may only have 1 spouse or domestic partner (not both)
- An employee may have an unlimited number of children
- Able to calculate and view a paycheck for an employee given the following rules:
	 - 26 paychecks per year with deductions spread as evenly as possible on each paycheck.
	 - Employees have a base cost of $1,000 per month (for benefits)
	 - Each dependent represents an additional $600 cost per month (for benefits)
	 - Employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs.
	 - Dependents that are over 50 years old will incur an additional $200 per month

## Technologies Used
- .NET 6
- Sqlite with Entity Framework Core for lightweight and simple data persistence
- XUnit and Moq for testing and mocking

## API Project Organization
I chose to implement the API using a service layer and the repository pattern since they are common and well known. Here are some additional folder used to help organize the project
- **Data** - Holds the EF core DbContext to make calls to the Sqlite database.
- **Extensions** - Holds extension methods currently used to setup the dependency injection of services and repositories and sqlite database
- **Services** - Holds the services and their interfaces
- **Repository** - Holds the repositories and their interfaces