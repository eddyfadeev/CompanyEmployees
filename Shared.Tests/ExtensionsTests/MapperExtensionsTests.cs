using Entities.Models;
using Shared.DTO;
using Shared.Extensions;

namespace Shared.Tests.ExtensionsTests;

public class MapperExtensionsTests
{
    private Employee _expectedEmployeeEntity;
    private Company _expectedCompanyEntity;

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
        
        _expectedCompanyEntity = new Company
        {
            Id = Guid.NewGuid(),
            Name = "ABC Company",
            Address = "123 Street, City",
            Country = "USA",
            Employees = [ _expectedEmployeeEntity ]
        };
    }

    #region MapToEmployeeForUpdateDto Tests
    
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

    #endregion
    
    #region Update Employee Entity Tests
    
    [Test]
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedDtoIsValid()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedNameIsNull()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedPositionIsNull()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedNameIsEmpty()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedPositionIsEmpty()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenAgeIsZero()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntityWithNotChangedAge_WhenAgeIsNegative()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntityWithNotChangedAge_WhenAgeIsAbove199()
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
    public void Employee_UpdateEntity_ShouldReturnUpdatedEntity_WhenAgeIs199()
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
    
    #endregion

    #region Update Company Entity Tests
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedDtoIsValid()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            "UpdatedAddress", 
            "UpdatedCountry",
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Country = testCompanyDto.Country;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedNameIsNull_NameUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            Name: null, 
            "UpdatedAddress", 
            "UpdatedCountry",
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Country = testCompanyDto.Country;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedAddressIsNull_AddressUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            Address: null, 
            "UpdatedCountry",
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Country = testCompanyDto.Country;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedCountryIsNull_CountryUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            "UpdatedAddress", 
            Country: null,
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedEmployeesIsNull_EmployeesUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            "UpdatedAddress", 
            "UpdatedCountry",
            Employees: null
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Country = testCompanyDto.Country;

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedNameIsEmpty_NameUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            Name: string.Empty, 
            "UpdatedAddress", 
            "UpdatedCountry",
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Country = testCompanyDto.Country;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedAddressIsEmpty_AddressUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            Address: string.Empty, 
            "UpdatedCountry",
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Country = testCompanyDto.Country;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedCountryIsEmpty_CountryUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            "UpdatedAddress", 
            Country: string.Empty,
            Employees:
            [
                new EmployeeForCreationDto{ Name = "Test1", Age = 1, Position = "Test1" }, 
                new EmployeeForCreationDto { Name = "Test2", Age = 2, Position = "Test2" }
            ]
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Employees = testCompanyDto.Employees.Select(e => e.MapToEntity()).ToList();

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }
    
    [Test]
    public void Company_UpdateEntity_ShouldReturnUpdatedEntity_WhenPassedEmployeesIsEmpty_EmployeesUnchanged()
    {
        var testCompanyEntity = new Company
        {
            Id = _expectedCompanyEntity.Id,
            Name = _expectedCompanyEntity.Name,
            Address = _expectedCompanyEntity.Address,
            Country = _expectedCompanyEntity.Country,
            Employees = _expectedCompanyEntity.Employees
        };
        
        var testCompanyDto = new CompanyForUpdateDto(
            "UpdatedName", 
            "UpdatedAddress", 
            "UpdatedCountry",
            Employees: []
        );
        
        _expectedCompanyEntity.Name = testCompanyDto.Name;
        _expectedCompanyEntity.Address = testCompanyDto.Address;
        _expectedCompanyEntity.Country = testCompanyDto.Country;

        var result = testCompanyEntity.UpdateEntity(testCompanyDto);
        
        Assert.That(result, Is.EqualTo(_expectedCompanyEntity));
    }

    #endregion

    #region Patch Emplloyee Entity Tests

    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenPassedDtoIsValid()
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

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenPassedNameIsNull_OnlyNameIsNull()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto(Name: null, 99, "UpdatedPosition");
        _expectedEmployeeEntity.Name = string.Empty;
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenPassedPositionIsNull_OnlyPositionIsNull()
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
        _expectedEmployeeEntity.Position = string.Empty;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenPassedNameIsEmpty_OnlyNameIsEmpty()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto(string.Empty, 99, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenPassedPositionIsEmpty_OnlyPositionIsEmpty()
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
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenAgeIsZero()
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
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntityWithAgeZero_WhenAgeIsNegative()
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
        _expectedEmployeeEntity.Age = 0;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    [Test]
    public void Employee_PatchEntity_ShouldReturnPatchedEntity_WhenAgeIsPositive()
    {
        var testEmployeeEntity = new Employee
        {
            Id = _expectedEmployeeEntity.Id,
            Name = _expectedEmployeeEntity.Name,
            Age = _expectedEmployeeEntity.Age,
            Position = _expectedEmployeeEntity.Position
        };
        
        var testEmployeeDto = new EmployeeForUpdateDto("UpdatedName", 10, "UpdatedPosition");
        _expectedEmployeeEntity.Name = testEmployeeDto.Name;
        _expectedEmployeeEntity.Age = testEmployeeDto.Age;
        _expectedEmployeeEntity.Position = testEmployeeDto.Position;

        var result = testEmployeeEntity.PatchEntity(testEmployeeDto);
        
        Assert.That(result, Is.EqualTo(_expectedEmployeeEntity));
    }
    
    #endregion
}