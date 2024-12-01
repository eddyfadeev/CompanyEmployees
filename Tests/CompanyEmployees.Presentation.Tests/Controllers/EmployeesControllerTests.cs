using CompanyEmployees.Presentation.Controllers;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Tests.Controllers;

public class EmployeesControllerTests
{
    private Mock<IEmployeeService> _mockEmployeeService;
    private EmployeesController _controller;

    [SetUp]
    public void Setup()
    {
        var mockServiceManager = new Mock<IServiceManager>();
        _mockEmployeeService = new Mock<IEmployeeService>();

        mockServiceManager.Setup(sm => sm.EmployeeService).Returns(_mockEmployeeService.Object);

        _controller = new EmployeesController(mockServiceManager.Object);
    }
    
    #region Get Employees For Company Tests
    
    [Test]
    public async Task GetEmployeesForCompany_ReturnsOkResult()
    {
        var employees = new List<EmployeeDto>
        {
            new () { Id = Guid.NewGuid(), Name = "Test Name 1" },
            new () { Id = Guid.NewGuid(), Name = "Test Name 2" }
        };

        _mockEmployeeService
            .Setup(s => s.GetEmployees(It.IsAny<Guid>(), It.IsAny<EmployeeParameters>(), It.IsAny<bool>()))
            .ReturnsAsync(employees);

        var result = await _controller.GetEmployeesForCompany(Guid.NewGuid(), new EmployeeParameters());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
    
    [Test]
    public async Task GetEmployeesForCompany_ReturnsOkResult_WithListOfEmployeeDtos()
    {
        var expected = new List<EmployeeDto>
        {
            new () { Id = Guid.NewGuid(), Name = "Test Name 1" },
            new () { Id = Guid.NewGuid(), Name = "Test Name 2" }
        };

        _mockEmployeeService
            .Setup(s => s.GetEmployees(It.IsAny<Guid>(), It.IsAny<EmployeeParameters>(), It.IsAny<bool>()))
            .ReturnsAsync(expected);

        var result = await _controller.GetEmployeesForCompany(Guid.NewGuid(), new EmployeeParameters());

        Assert.That((result as OkObjectResult)?.Value, Is.EquivalentTo(expected));
    }
    
    #endregion

    #region Get Employee For Company Tests
    
    [Test]
    public async Task GetEmployeeForCompany_ReturnsOkResult()
    {
        var employee = new EmployeeDto
        {
            Id = Guid.NewGuid(), 
            Name = "Test Name",
            Age = 18,
            Position = "Test Position"
        };

        _mockEmployeeService
            .Setup(s => s.GetEmployee(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync(employee);

        var result = await _controller.GetEmployeeForCompany(Guid.NewGuid(), employee.Id);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
    
    [Test]
    public async Task GetEmployeeForCompany_ReturnsOkResult_WithEmployeeDtos()
    {
        var expected = new EmployeeDto
        {
            Id = Guid.NewGuid(), 
            Name = "Test Name",
            Age = 18,
            Position = "Test Position"
        };

        _mockEmployeeService
            .Setup(s => s.GetEmployee(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync(expected);

        var result = await _controller.GetEmployeeForCompany(Guid.NewGuid(), expected.Id);

        Assert.That((result as OkObjectResult)?.Value, Is.EqualTo(expected));
    }

    #endregion

    #region Create Employee For Company Tests
    
    [Test]
    public async Task CreateEmployeeForCompany_ReturnsCreatedAtRouteResult()
    {
        var employee = new EmployeeForCreationDto
        {
            Name = "Test",
            Age = 18,
            Position = "QA"
        };

        _mockEmployeeService
            .Setup(s => s.CreateEmployeeForCompany(It.IsAny<Guid>(), It.IsAny<EmployeeForCreationDto>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new EmployeeDto
            {
                Age = employee.Age,
                Name = employee.Name,
                Position = employee.Position,
            });

        var result = await _controller.CreateEmployeeForCompany(companyId: Guid.NewGuid(), employee);
        
        Assert.That(result, Is.InstanceOf<CreatedAtRouteResult>());
    }
    
    [Test]
    public async Task CreateEmployeeForCompany_ReturnsEmployeeDto()
    {
        var employee = new EmployeeForCreationDto
        {
            Name = "Test",
            Age = 18,
            Position = "QA"
        };

        _mockEmployeeService
            .Setup(s => s.CreateEmployeeForCompany(It.IsAny<Guid>(), It.IsAny<EmployeeForCreationDto>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new EmployeeDto
            {
                Age = employee.Age,
                Name = employee.Name,
                Position = employee.Position,
            });

        var result = await _controller.CreateEmployeeForCompany(companyId: Guid.NewGuid(), employee);
        
        Assert.That((result as CreatedAtRouteResult)?.Value, Is.InstanceOf<EmployeeDto>());
    }
    
    [Test]
    public async Task CreateEmployeeForCompany_ReturnsCorrectEmployeeDto()
    {
        var employee = new EmployeeForCreationDto
        {
            Name = "Test",
            Age = 18,
            Position = "QA"
        };

        var expected = new EmployeeDto
        {
            Id = Guid.Empty,
            Name = employee.Name,
            Age = employee.Age,
            Position = employee.Position
        };

        _mockEmployeeService
            .Setup(s => s.CreateEmployeeForCompany(It.IsAny<Guid>(), It.IsAny<EmployeeForCreationDto>(),
                It.IsAny<bool>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateEmployeeForCompany(companyId: Guid.NewGuid(), employee);
        
        Assert.That((result as CreatedAtRouteResult)?.Value, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task CreateEmployeeForCompany_ReturnsCorrectRouteName()
    {
        var employee = new EmployeeForCreationDto
        {
            Name = "Test",
            Age = 18,
            Position = "QA"
        };

        var expected = "GetEmployeeForCompany";

        _mockEmployeeService
            .Setup(s => s.CreateEmployeeForCompany(It.IsAny<Guid>(), It.IsAny<EmployeeForCreationDto>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new EmployeeDto()
            {
                Id = Guid.Empty,
                Age = employee.Age,
                Name = employee.Name,
                Position = employee.Position,
            });

        var result = await _controller.CreateEmployeeForCompany(companyId: Guid.NewGuid(), employee);
        
        Assert.That((result as CreatedAtRouteResult)?.RouteName, Is.EqualTo(expected));
    }

    #endregion

    #region Delete Employee For Company Tests

    [Test]
    public async Task DeleteEmployeeForCompany_ReturnsNoContent()
    {
        var result = await _controller.DeleteEmployeeForCompany(companyId: Guid.NewGuid(), employeeId: Guid.NewGuid());

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    #endregion

    #region Update Employee For Company Tests
    
    [Test]
    public async Task UpdateEmployeeForCompany_ReturnsNoContentResult()
    {
        var result = await _controller.UpdateEmployeeForCompany(companyId: Guid.NewGuid(), employeeId: Guid.NewGuid(),
            employee: new EmployeeForUpdateDto());
        
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    #endregion

    #region Partial Update Employee For Company Tests

    [Test]
    public async Task PartialUpdateEmployeeForCompany_ReturnsBadRequest_WhenPassedEmployeeIsNull()
    {
        var result =
            await _controller.PartialUpdateEmployeeForCompany(companyId: Guid.NewGuid(), employeeId: Guid.NewGuid(), null);
        
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task PartialUpdateEmployeeForCompany_ReturnsUnprocessableEntity_WhenModelStateIsInvalid()
    {
        var employeeToPatch = new EmployeeForUpdateDto
        {
            Name = "Test",
            Age = 18,
            Position = "QA"
        };

        var employee = new Employee
        {
            Name = employeeToPatch.Name,
            Age = employeeToPatch.Age,
            Position = employeeToPatch.Position
        };
        
        var patchDoc = new JsonPatchDocument<EmployeeForUpdateDto>();
        var mockValidator = new Mock<IObjectModelValidator>();

        _mockEmployeeService
            .Setup(s => s.GetEmployeeForPatch(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync((employeeToPatch, employee));

        _controller.ObjectValidator = mockValidator.Object;
        
        _controller.ModelState.AddModelError("Name", "Invalid name");

        var result = await _controller.PartialUpdateEmployeeForCompany(
            companyId: Guid.NewGuid(),
            employeeId: Guid.NewGuid(),
            patchDoc: patchDoc);

        Assert.That(result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }
    
    [Test]
    public async Task PartialUpdateEmployeeForCompany_ReturnsNoContentResult()
    {
        var employeeToPatch = new EmployeeForUpdateDto
        {
            Name = "Test",
            Age = 18,
            Position = "QA"
        };

        var employee = new Employee
        {
            Name = employeeToPatch.Name,
            Age = employeeToPatch.Age,
            Position = employeeToPatch.Position
        };

        _mockEmployeeService
            .Setup(s => s.GetEmployeeForPatch(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync((employeeToPatch, employee));

        var mockValidator = new Mock<IObjectModelValidator>();

        _controller.ObjectValidator = mockValidator.Object;

        var patchDoc = new JsonPatchDocument<EmployeeForUpdateDto>();

        var result = await _controller.PartialUpdateEmployeeForCompany(
            companyId: Guid.NewGuid(),
            employeeId: Guid.NewGuid(),
            patchDoc: patchDoc);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    #endregion
}