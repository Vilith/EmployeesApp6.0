using EmployeesApp.Application.Employees.Interfaces;
using EmployeesApp.Domain.Entities;
using EmployeesApp.Infrastructure.Persistance.Repositories;
using EmployeesApp.Web.Controllers;
using EmployeesApp.Web.Views.Employees;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

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
        [InlineData("", "name", 4, false)]
        [InlineData("mail@mail.com", "", 4, false)]
        [InlineData("other@other.com", "other", 4, true)]
        public void Create_WithValidViewModel_ReturnsViewResult(string email, string Name, int botcheck, bool expected)
        {
            //Arrange
            var viewModel = new CreateVM { Email = email, Name = Name, BotCheck = botcheck };
            var mockService = new Mock<IEmployeeService>();
            var controller = new EmployeesController(mockService.Object);

            ValidateModel(viewModel, controller);

            //Act
            var result = controller.Create(viewModel);

            //Assert
            if (expected)
            {
                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
                mockService.Verify(s => s.Add(It.Is<Employee>(e =>
                    e.Email == email &&
                    e.Name == Name)), Times.Once);
            }
            else
            {
                var view = Assert.IsType<ViewResult>(result);
                Assert.Null(view.ViewName);
                mockService.Verify(s => s.Add(It.IsAny<Employee>()), Times.Never);
            }
        }

        [Fact]
        public void Details_WithId_ReturnsViewResult()
        {
            //Arrange
            var employeeService = new Mock<IEmployeeService>();
            var employeeController = new EmployeesController(employeeService.Object);
            var employee = new Employee
            {
                Name = "Alice",
                Email = "alice@example.com"
            };

            employeeService.Setup(service => service.GetById(1))
                .Returns(employee);

            //Act

            var result = employeeController.Details(1);

            //Assert
            Assert.IsType<ViewResult>(result);
            employeeService.Verify(s => s.GetById(1), Times.Once);
        }

        //Helpfunction for validation of the model state 
        #region[Help-function]
        private static void ValidateModel(object model, Controller controller) //Alt+enter helped us here
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    controller.ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }
        }
        #endregion 
    }
}
