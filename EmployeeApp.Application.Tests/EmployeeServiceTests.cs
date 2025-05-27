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
            var catchEmployee = new Employee();
            var mockRepo = new Mock<IEmployeeRepository>();

            // Catch what's being sent to .Add()
            mockRepo
                .Setup(r => r.Add(It.IsAny<Employee>()))
                .Callback<Employee>(e => catchEmployee = e);

            var service = new EmployeeService(mockRepo.Object);
            var input = new Employee
            {
                Name = "lisa",
                Email = "lisa@ajax.com"
            };

            // Act
            service.Add(input);

            // Assert
            Assert.Equal("Lisa", catchEmployee.Name);
            Assert.Equal("lisa@ajax.com", catchEmployee.Email);
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
