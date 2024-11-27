using Entities.Models;
using Shared.DTO;
using Shared.Extensions;

namespace Shared.Tests.ExtensionsTests;

public class MapperExtensionsTests
{
    private Employee _expectedEmployeeEntity;
    private EmployeeForUpdateDto _employeeForUpdateDto;

    [SetUp]
    public void Setup()
    {
        _expectedEmployeeEntity = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Age = 30,
            Position = "Developer"
        };

        _employeeForUpdateDto = new EmployeeForUpdateDto(_expectedEmployeeEntity.Name, _expectedEmployeeEntity.Age, _expectedEmployeeEntity.Position);
    }

    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenInputIsValid()
    {
        var expected = new EmployeeForUpdateDto(
            _expectedEmployeeEntity.Name, _expectedEmployeeEntity.Age, _expectedEmployeeEntity.Position);

        var result = _expectedEmployeeEntity.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenNameIsEmpty()
    {
        _expectedEmployeeEntity.Name = string.Empty;
        var expected = new EmployeeForUpdateDto(
            _expectedEmployeeEntity.Name, _expectedEmployeeEntity.Age, _expectedEmployeeEntity.Position);

        var result = _expectedEmployeeEntity.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenNameIsNull()
    {
        _expectedEmployeeEntity.Name = null;
        var expected = new EmployeeForUpdateDto(
            Name:string.Empty, _expectedEmployeeEntity.Age, _expectedEmployeeEntity.Position);

        var result = _expectedEmployeeEntity.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenPositionIsEmpty()
    {
        _expectedEmployeeEntity.Position = string.Empty;
        var expected = new EmployeeForUpdateDto(
            _expectedEmployeeEntity.Name, _expectedEmployeeEntity.Age, _expectedEmployeeEntity.Position);

        var result = _expectedEmployeeEntity.MapToEmployeeForUpdateDto();
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MapToEmployeeForUpdateDto_ShouldReturnCorrectDto_WhenPositionIsNull()
    {
        _expectedEmployeeEntity.Position = null;
        var expected = new EmployeeForUpdateDto(
            _expectedEmployeeEntity.Name, _expectedEmployeeEntity.Age, Position:string.Empty);

        var result = _expectedEmployeeEntity.MapToEmployeeForUpdateDto();
        
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

    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedDtoIsValid()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 99, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedNameIsNull()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto(Name: null, 99, "UpdatedPosition");
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedPositionIsNull()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 99, Position: null);
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedNameIsEmpty()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto(Name: string.Empty, 99, "UpdatedPosition");
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedPositionIsEmpty()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 99, Position: string.Empty);
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenAgeIsZero()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 0, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenAgeIsNegative()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", -1, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenAgeIsAbove199()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 200, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void UpdateEntity_ShouldReturnUpdatedEntity_WhenAgeIs199()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 199, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.UpdateEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
}