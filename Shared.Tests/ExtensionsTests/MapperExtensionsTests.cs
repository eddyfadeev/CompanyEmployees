using Entities.Models;
using Shared.DTO;
using Shared.Extensions;

namespace Shared.Tests.ExtensionsTests;

public class MapperExtensionsTests
{
    private Employee _employee;
    private EmployeeForUpdateDto _employeeForUpdateDto;

    [SetUp]
    public void Setup()
    {
        _employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Age = 30,
            Position = "Developer"
        };

        _employeeForUpdateDto = new EmployeeForUpdateDto(_employee.Name, _employee.Age, _employee.Position);
    }

    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenInputIsValid()
    {
        var expected = new EmployeeForUpdateDto(
            _employee.Name, _employee.Age, _employee.Position);

        var result = _employee.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenNameIsEmpty()
    {
        _employee.Name = string.Empty;
        var expected = new EmployeeForUpdateDto(
            _employee.Name, _employee.Age, _employee.Position);

        var result = _employee.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenNameIsNull()
    {
        _employee.Name = null;
        var expected = new EmployeeForUpdateDto(
            Name:string.Empty, _employee.Age, _employee.Position);

        var result = _employee.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenPositionIsEmpty()
    {
        _employee.Position = string.Empty;
        var expected = new EmployeeForUpdateDto(
            _employee.Name, _employee.Age, _employee.Position);

        var result = _employee.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenPositionIsNull()
    {
        _employee.Position = null;
        var expected = new EmployeeForUpdateDto(
            _employee.Name, _employee.Age, Position:string.Empty);

        var result = _employee.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_ShouldReturnCorrectEntity_WhenInputIsValid()
    {
        var expected = new Employee
        {
            Name = _employeeForUpdateDto.Name,
            Age = _employeeForUpdateDto.Age,
            Position = _employeeForUpdateDto.Position
        };

        var result = _employeeForUpdateDto.MapToEntity();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_ShouldReturnCorrectEntity_WhenNameIsEmpty()
    {
        var testEmployee = _employeeForUpdateDto with { Name = string.Empty };

        var expected = new Employee
        {
            Name = testEmployee.Name,
            Age = testEmployee.Age,
            Position = testEmployee.Position
        };

        var result = testEmployee.MapToEntity();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_ShouldReturnCorrectEntity_WhenNameIsNull()
    {
        var testEmployee = _employeeForUpdateDto with { Name = null };

        var expected = new Employee
        {
            Name = string.Empty,
            Age = testEmployee.Age,
            Position = testEmployee.Position
        };

        var result = testEmployee.MapToEntity();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_ShouldReturnCorrectEntity_WhenPositionIsEmpty()
    {
        var testEmployee = _employeeForUpdateDto with { Position = string.Empty };

        var expected = new Employee
        {
            Name = testEmployee.Name,
            Age = testEmployee.Age,
            Position = testEmployee.Position
        };

        var result = testEmployee.MapToEntity();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_ShouldReturnCorrectEntity_WhenPositionIsNull()
    {
        var testEmployee = _employeeForUpdateDto with { Position = null };

        var expected = new Employee
        {
            Name = testEmployee.Name,
            Age = testEmployee.Age,
            Position = string.Empty
        };

        var result = testEmployee.MapToEntity();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_PositionShouldBeNotNull_WhenPassedPositionIsNull()
    {
        var testEmployee = _employeeForUpdateDto with { Position = null };

        var result = testEmployee.MapToEntity();
        
        Assert.That(result.Position, Is.Not.Null);
    }
    
    [Test]
    public void MapFromEmployeeForUpdateDtoToEntity_NameShouldBeNotNull_WhenPassedNameIsNull()
    {
        var testEmployee = _employeeForUpdateDto with { Name = null };

        var result = testEmployee.MapToEntity();
        
        Assert.That(result.Name, Is.Not.Null);
    }
}