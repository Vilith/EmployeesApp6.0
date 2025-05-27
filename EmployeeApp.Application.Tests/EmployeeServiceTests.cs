using EmployeesApp.Application.Employees.Interfaces;
using EmployeesApp.Application.Employees.Services;
using EmployeesApp.Domain.Entities;
using Moq;

namespace EmployeeApp.Application.Tests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void AddEmployee_WithRightCredentials_WillAddEmployeeToList()
        {
            // Arrange
            var employeeRepository = new Mock<IEmployeeRepository>();
            //employeeRepository.Setup(o => o.Add(new Employee {Name="Ronald", Email="ronald@mcdonnald.com" }));
            var employeeService = new EmployeeService(employeeRepository.Object);

            // Act
            Employee newEmployee = new Employee { Name="Ronald", Email="ronald@mcdonnald.com" };
            employeeService.Add(newEmployee);

            // Assert
            var employeeLista = employeeService.GetAll();
            Assert.Contains(newEmployee, employeeLista);
                //repository.verify
        }

        //public void Add(Employee employee)
        //{

        //}

        //private static string ToInitalCapital(string s) =>
        //    $"{s[..1].ToUpper()}{s[1..]}";


        //public Employee[] GetAll() => [.. employeeRepository.GetAll().OrderBy(e => e.Name)];

        //public Employee? GetById(int id)
        //{
        //}

        //public bool CheckIsVIP(Employee employee) =>
        //    employee.Email.StartsWith("ANDERS", StringComparison.CurrentCultureIgnoreCase);
        //}
    }
}
