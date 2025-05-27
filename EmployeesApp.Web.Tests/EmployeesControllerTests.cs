using EmployeesApp.Application.Employees.Interfaces;
using EmployeesApp.Domain.Entities;
using EmployeesApp.Web.Controllers;
using EmployeesApp.Web.Views.Employees;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeesApp.Web.Tests
{
    public class EmployeesControllerTests
    {
        [Fact]
        public void Index_NoParams_ReturnsViewResult()
        {
            //Arrange
            var employeeService = new Mock<IEmployeeService>();

            employeeService.Setup(service => service.GetAll())
                .Returns([
                    new Employee{Email = "gmail@gmail.com", Name = "Pär"},
                    new Employee{Email = "email@email.com", Name = "Name"},
                    new Employee{Email = "hotmail@hotmail.com", Name = "Namn"}
                ]);

            var employeeController = new EmployeesController(employeeService.Object);
            //Act

            var result = employeeController.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("email@email.com", "namn", 3, false)]
        [InlineData("other@other.com", "other", 4, true)]
        public void Create_WithValidViewModel_ReturnsViewResult(string email, string Name, int botcheck, bool expected)
        {
            //Arrange
            var viewModel = new CreateVM { Email = email, Name = Name, BotCheck = botcheck };
            var employeeService = new Mock<IEmployeeService>();

            var employeeController = new EmployeesController(employeeService.Object);

            if (!expected)
            {
                employeeController.ModelState.AddModelError("false", "ViewModel invalid");
            }
            //Act
            var result = employeeController.Create(viewModel);


            //Assert
            //Assert.Equal(viewModel, result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
