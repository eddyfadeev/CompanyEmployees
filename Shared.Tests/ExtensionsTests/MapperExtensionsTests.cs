using Entities.Models;
using Shared.DTO;
using Shared.Extensions;

namespace Shared.Tests.ExtensionsTests;

public class MapperExtensionsTests
{
    private Employee _employee;

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
            string.Empty, _employee.Age, _employee.Position);

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
            _employee.Name, _employee.Age, string.Empty);

        var result = _employee.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
}